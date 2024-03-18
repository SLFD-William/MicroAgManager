using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

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

        public async static Task<DateTime?> GetNextChoreDueDate(IFrontEndDbContext context, IScheduledDuty completedScheduledDuty)
        {
            if (completedScheduledDuty.ScheduleSource != ScheduledDutySourceConstants.Chore || !completedScheduledDuty.CompletedOn.HasValue) return null;
            
            var chore = await context.Chores.FindAsync(completedScheduledDuty.ScheduleSourceId);
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
                   s.CompletedOn.HasValue &&
                   s.CompletedOn >= DateTime.Now.AddSeconds(-lookback)).ToListAsync();

            if (chore.PerScalar >  1 && completedDutyChores.Count % chore.PerScalar == 0)
                return (completedDate.Date + chore.DueByTime).AddSeconds((completedDutyChores.Count + 1)*per);

            return newDate.Date + chore.DueByTime;

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
