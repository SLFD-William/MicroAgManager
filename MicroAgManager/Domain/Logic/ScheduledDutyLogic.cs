﻿using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class ScheduledDutyLogic
    {
        public static IQueryable<ScheduledDutyModel> OpenScheduledDutyQuery(DbContext genericContext)
        {
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            var query = context.ScheduledDuties.Include(p => p.Duty).Where(d => !d.CompletedOn.HasValue).ToList();
            foreach (var sd in query)
            {
                sd.PopulateDynamicRelations(context as DbContext);
                sd.Duty.PopulateDynamicRelations(context as DbContext);
            }
            return query.AsQueryable();
        }
        public static string GetRecipientHref(IScheduledDuty? duty) => $"/{duty?.Recipient}?{duty?.Recipient}Id={duty?.RecipientId}";
        public static string GetRecordHref(IScheduledDuty? duty)
        {
            if(duty.RecordId<1) return string.Empty;
            if (duty.Record == ScheduledDutyRecordConstants.TreatmentRecord)
                return $"/Ancillaries/TreatmentRecords?TreatmentRecordId={duty.RecordId}";
            if (duty.Record == ScheduledDutyRecordConstants.Measurement)
                return $"/Ancillaries/Measurements?MeasurementId={duty.RecordId}";
            if (duty.Record == ScheduledDutyRecordConstants.Registration)
                return $"/Ancillaries/Registrations?RegistrationId={duty.RecordId}";
            if (duty.Record == ScheduledDutyRecordConstants.BreedingRecord)
                return $"/Ancillaries/BreedingRecords?BreedingRecordId={duty.RecordId}";
            return string.Empty;
        }
        
        public static string GetSourceIcon(IScheduledDuty scheduledDuty)
        {
            if (!(scheduledDuty is IScheduledDuty)) return null;
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Milestone)
                return "fa-star-of-life";
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Event)
                return "fa-calendar-days";
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore)
                return "fa-person-digging";
            return null;
        }
        public async static Task<BaseModel?> GetSourceRecord(DbContext genericContext, IScheduledDuty scheduledDuty)
        {
            if (!(scheduledDuty is IScheduledDuty)) return null;
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Milestone)
                return await context.Milestones.FindAsync(scheduledDuty.ScheduleSourceId);
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Event)
                return await context.Events.FindAsync(scheduledDuty.ScheduleSourceId);
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore)
                return await context.Chores.FindAsync(scheduledDuty.ScheduleSourceId);
            return null;

        }
        public static List<KeyValuePair<long, string>> SourceIds(DbContext genericContext, IScheduledDuty scheduledDuty)
        {
            if (!(scheduledDuty is IScheduledDuty)) return new List<KeyValuePair<long, string>>();
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null; if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Milestone)
                return context.Milestones.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore)
                return context.Chores.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (scheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Event)
                return context.Events.OrderByDescending(a => a.StartDate).ThenBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();

            return new List<KeyValuePair<long, string>>();
        }
        public async static Task<DateTime?> GetNextChoreDueDate(DbContext genericContext, IScheduledDuty completedScheduledDuty)
        {
            if (completedScheduledDuty.ScheduleSource != ScheduledDutySourceConstants.Chore || !completedScheduledDuty.CompletedOn.HasValue) return null;
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;

            var chore = await GetSourceRecord(context as DbContext, completedScheduledDuty) as ChoreModel;
            if (chore?.Enabled != true) return null;
            var completedDate = completedScheduledDuty.CompletedOn.Value;

            var per = chore.PerUnit?.ConversionFactorToSIUnit / (double)chore.PerScalar ?? 0;
            var every = chore.EveryUnit?.ConversionFactorToSIUnit * (double)chore.EveryScalar ?? 0;
            var lookback = chore.PerUnit?.ConversionFactorToSIUnit ?? 0;
            var newTime = completedDate.AddSeconds(per);
            var newDate = completedDate.AddSeconds(every);


            if (newTime == newDate) return newDate.Date + chore.DueByTime;

            var completedDutyChores = await context.ScheduledDuties.Where(s =>
                   s.ScheduleSource == completedScheduledDuty.ScheduleSource &&
                   s.ScheduleSourceId == completedScheduledDuty.ScheduleSourceId &&
                   s.DutyId == completedScheduledDuty.DutyId &&
                   s.RecipientId == completedScheduledDuty.RecipientId &&
                   s.CompletedOn.HasValue &&
                   s.CompletedOn >= DateTime.Now.AddSeconds(-lookback)).ToListAsync();

            if (chore.PerScalar > 1 && completedDutyChores.Count % chore.PerScalar == 0)
                return (completedDate.Date + chore.DueByTime).AddSeconds((completedDutyChores.Count + 1) * per);

            return newDate.Date + chore.DueByTime;

        }
        public async static Task<DateTime?> GetNextFreqAndDurationDueDate(DbContext genericContext, IScheduledDuty completedScheduledDuty)
        {
            if (completedScheduledDuty.ScheduleSource == ScheduledDutySourceConstants.Chore || !completedScheduledDuty.CompletedOn.HasValue) return null;
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            var duty = await context.Duties.FindAsync(completedScheduledDuty.DutyId);
            var commandItem = await DutyLogic.GetCommandItem(context as DbContext, duty) as IHasFrequencyAndDuration;
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

            var startDate = DateTime.Now;
            var source = await GetSourceRecord(context as DbContext, completedScheduledDuty);
            if (source is EventModel)
                startDate = ((EventModel)source).StartDate;

            var recordQuery = DutyLogic.GetCommandRecordPerformedAfter(context as DbContext, duty, startDate,completedScheduledDuty.RecipientId);
            var completedRecords = await recordQuery?.ToListAsync() ?? new List<BaseModel>();

            var completedDate = completedScheduledDuty.CompletedOn.Value;

            var per = perUnit?.ConversionFactorToSIUnit / (double)commandItem.PerScalar ?? 0;
            var every = everyUnit?.ConversionFactorToSIUnit * (double)commandItem.EveryScalar ?? 0;
            var duration = durationUnit?.ConversionFactorToSIUnit * (double)commandItem.DurationScalar ?? 0;
            var lookback = perUnit?.ConversionFactorToSIUnit ?? 0;
            var completedPer = 0;

            if (duty.Command == DutyCommandConstants.Treatment)
                completedPer = completedRecords.Where(t => ((TreatmentRecordModel)t).DatePerformed >= DateTime.Now.AddSeconds(-lookback)).Count();

            var totalNeeded = (every / per) * (duration / every);

            if (totalNeeded > completedRecords.Count + 1 && commandItem.PerScalar > 1 && completedPer + 1 < commandItem.PerScalar)
                return (completedDate.Date).AddSeconds((completedPer + 1) * per);

            if (totalNeeded > completedRecords.Count + 1)
                return (completedDate.Date).AddSeconds(every);

            return null;
        }
        public async static Task<ICreateScheduledDuty?> OnCompleted(DbContext genericContext, IHasReschedule command, IScheduledDuty duty)
        {
            if (!(command.Reschedule == true && command.RescheduleDueOn.HasValue && duty.CompletedOn.HasValue)) return null;
            var context = genericContext as IMicroAgManagementDbContext;
            if (context is null) return null;

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
        public static IQueryable<ScheduledDutyModel> CommandDuties(DbContext genericContext, string command)
        {
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            return context.ScheduledDuties.Include(s => s.Duty)
                                .Where(s => !s.CompletedOn.HasValue && s.Duty.Command == command);
        }
       
    }
}
