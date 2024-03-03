using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Logic
{
    public class CreateScheduledDuty : ICreateScheduledDuty
    {
        public Guid CreatedBy { get; set; }
        public ScheduledDutyModel ScheduledDuty { get; set ; }
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
        public async static Task<ICreateScheduledDuty> RescheduleDuty(IMicroAgManagementDbContext context, long scheduledDutyId, DateTime RescheduledDue)
        {
            var currentDuty = await context.ScheduledDuties.FindAsync(scheduledDutyId);
            if (currentDuty is null) return null;

            var newDuty = currentDuty.Clone() as ScheduledDuty;
            newDuty.DueOn = RescheduledDue;
            newDuty.CompletedBy = null;
            newDuty.CompletedOn = null;
            newDuty.Dismissed = false;
            newDuty.RecordId = null;
            return new CreateScheduledDuty { CreatedBy = newDuty.CreatedBy, ScheduledDuty =ScheduledDutyModel.Create(newDuty) };

        }
    }
}
