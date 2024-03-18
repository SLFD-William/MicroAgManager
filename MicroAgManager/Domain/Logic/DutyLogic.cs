using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.NetworkInformation;

namespace Domain.Logic
{
    public class CreateScheduledDuty : ICreateScheduledDuty, IBaseCommand
    {
        public Guid CreatedBy { get; set; }
        public ScheduledDutyModel ScheduledDuty { get; set ; }
        public Guid ModifiedBy { get; set; }
        public Guid TenantId { get; set; }
    }
    public static class DutyLogic
    {
        public static string GetRecordTypeFromCommand(IDuty duty)
        { 
            switch (duty.Command)
            {
                case nameof(DutyCommandConstants.Measurement):
                    return nameof(Measurement);
                case nameof(DutyCommandConstants.Treatment):
                    return nameof(TreatmentRecord);
                default:
                    return string.Empty;
            }
        }

        public async static Task<BaseModel?> GetDutyCommandItem(IFrontEndDbContext context, IDuty? duty)
        {
            if (!(duty is IDuty)) return null;
            if (duty.Command == DutyCommandConstants.Treatment)
                return await context.Treatments.FindAsync(duty.CommandId);
            if (duty.Command == DutyCommandConstants.Measurement)
                return await context.Measures.FindAsync(duty.CommandId);
            if (duty.Command == DutyCommandConstants.Registration)
                return await context.Registrars.FindAsync(duty.CommandId);
            return null;
        }
        public async static Task<BaseModel?> GetScheduledDutySourceRecord(IFrontEndDbContext context, IScheduledDuty scheduledDuty)
        {
            if (!(scheduledDuty is IScheduledDuty)) return null;
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Milestone)
                return await context.Milestones.FindAsync(scheduledDuty.ScheduleSourceId);
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Event)
                return await context.Events.FindAsync(scheduledDuty.ScheduleSourceId);
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore)
                return await context.Chores.FindAsync(scheduledDuty.ScheduleSourceId);
            return null;

        }

        public static IQueryable<BaseModel>? GetDutyCommandRecordPerformedAfter(IFrontEndDbContext context, IDuty? duty,DateTime startDate)
        {
            if (!(duty is IDuty)) return null;
            if (duty.Command == DutyCommandConstants.Treatment)
                return context.TreatmentRecords.Where(t=>t.DatePerformed>=startDate).AsQueryable();
            if (duty.Command == DutyCommandConstants.Measurement)
                return context.Measurements.Where(t => t.DatePerformed >= startDate).AsQueryable();
            if (duty.Command == DutyCommandConstants.Registration)
                return context.Registrations.Where(t => t.RegistrationDate >= startDate).AsQueryable();
            return null;
        }

        public static List<KeyValuePair<long, string>> SourceIds(IFrontEndDbContext context, IScheduledDuty scheduledDuty)
        {
            if (!(scheduledDuty is IScheduledDuty)) return new List<KeyValuePair<long, string>>();
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Milestone)
                return context.Milestones.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore)
                return context.Chores.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Event)
                return context.Events.OrderByDescending(a => a.StartDate).ThenBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();

            return new List<KeyValuePair<long, string>>();
        }
        public async static Task<DateTime?> GetNextChoreDueDate(IFrontEndDbContext context, IScheduledDuty completedScheduledDuty)
        {
            if (completedScheduledDuty.ScheduleSource != ScheduledDutySourceConstants.Chore || !completedScheduledDuty.CompletedOn.HasValue) return null;

            var chore = await GetScheduledDutySourceRecord(context, completedScheduledDuty) as ChoreModel;
            if(chore?.Enabled != true) return null;
            var completedDate = completedScheduledDuty.CompletedOn.Value;

            var per = chore.PerUnit?.ConversionFactorToSIUnit / (double)chore.PerScalar ?? 0;
            var every = chore.EveryUnit?.ConversionFactorToSIUnit * (double)chore.EveryScalar ?? 0;
            var lookback = chore.PerUnit?.ConversionFactorToSIUnit ?? 0;
            var newTime = completedDate.AddSeconds(per);
            var newDate = completedDate.AddSeconds(every );

         
            if (newTime == newDate) return newDate.Date + chore.DueByTime;

            var completedDutyChores= await context.ScheduledDuties.Where(s =>
                   s.ScheduleSource == completedScheduledDuty.ScheduleSource &&
                   s.ScheduleSourceId == completedScheduledDuty.ScheduleSourceId &&
                   s.DutyId == completedScheduledDuty.DutyId &&
                   s.RecipientId == completedScheduledDuty.RecipientId &&
                   s.CompletedOn.HasValue &&
                   s.CompletedOn >= DateTime.Now.AddSeconds(-lookback)).ToListAsync();

            if (chore.PerScalar >  1 && completedDutyChores.Count % chore.PerScalar == 0)
                return (completedDate.Date + chore.DueByTime).AddSeconds((completedDutyChores.Count + 1)*per);

            return newDate.Date + chore.DueByTime;

        }
        public async static Task<DateTime?> GetNextFreqAndDurationDueDate(IFrontEndDbContext context, IScheduledDuty completedScheduledDuty)
        {
            if (completedScheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore || !completedScheduledDuty.CompletedOn.HasValue) return null;
            var duty = await context.Duties.FindAsync(completedScheduledDuty.DutyId);
            var commandItem =await GetDutyCommandItem(context, duty) as IHasFrequencyAndDuration;
            if (!(commandItem is IHasFrequencyAndDuration)) return null;

            var perUnit = await context.Units.FindAsync(commandItem.PerUnitId);
            var everyUnit = await context.Units.FindAsync(commandItem.EveryUnitId);
            var durationUnit = await context.Units.FindAsync(commandItem.DurationUnitId);

            //one and done
            if (perUnit == everyUnit &&
                everyUnit == durationUnit &&
                commandItem.PerScalar == commandItem.EveryScalar &&
                commandItem.EveryScalar == commandItem.DurationScalar &&
                commandItem.DurationScalar == 1)
                return null;


            var startDate =DateTime.Now;
            var source = await GetScheduledDutySourceRecord(context, completedScheduledDuty);
            if(source is EventModel)
                startDate=((EventModel)source).StartDate;

            var recordQuery = GetDutyCommandRecordPerformedAfter(context, duty, startDate);
            var completedRecords = await recordQuery?.ToListAsync() ?? new List<BaseModel>();

            var completedDate = completedScheduledDuty.CompletedOn.Value;

            var per = perUnit?.ConversionFactorToSIUnit / (double)commandItem.PerScalar ?? 0;
            var every = everyUnit?.ConversionFactorToSIUnit * (double)commandItem.EveryScalar ?? 0;
            var duration = durationUnit?.ConversionFactorToSIUnit * (double)commandItem.DurationScalar ?? 0;
         
            
            var lookback = perUnit?.ConversionFactorToSIUnit ?? 0;
            var completedPer = 0;
            if (duty.Command == DutyCommandConstants.Treatment)
                completedPer= completedRecords.Where(t => ((TreatmentRecordModel)t).DatePerformed >= DateTime.Now.AddSeconds(-lookback)).Count();

            var totalNeeded = (every / per) * (duration / every);
            
            if(totalNeeded > completedRecords.Count + 1 && commandItem.PerScalar > 1 && completedPer+1 < commandItem.PerScalar)
                return (completedDate.Date).AddSeconds((completedPer + 1) * per);

            if (totalNeeded > completedRecords.Count + 1)
                return (completedDate.Date).AddSeconds(every);

           
            return null;

        }
        public async static Task<ICreateScheduledDuty?> OnScheduledDutyCompleted(IMicroAgManagementDbContext context, IHasReschedule command, IScheduledDuty duty)
        {
            if (!(command.Reschedule == true && command.RescheduleDueOn.HasValue && duty.CompletedOn.HasValue)) return null;
            var currentDuty = await context.ScheduledDuties.FindAsync(duty.Id);
            if (currentDuty is null) return null;

            var newDuty = (ScheduledDuty)currentDuty.Clone();
            newDuty.DueOn = command.RescheduleDueOn.Value;
            newDuty.CompletedBy = null;
            newDuty.CompletedOn = null;
            newDuty.Dismissed = false;
            newDuty.RecordId = null;
            return new CreateScheduledDuty
            {
                CreatedBy = newDuty.CreatedBy,
                ScheduledDuty = ScheduledDutyModel.Create(newDuty),
                TenantId = ((IBaseCommand)command).TenantId,
                ModifiedBy = ((IBaseCommand)command).ModifiedBy
            };

        }
        
    }
}
