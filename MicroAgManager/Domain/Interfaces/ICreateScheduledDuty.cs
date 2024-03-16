using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICreateScheduledDuty
    {
        public Guid CreatedBy { get ; set ; }
        public ScheduledDutyModel ScheduledDuty { get; set; }
    }
}
