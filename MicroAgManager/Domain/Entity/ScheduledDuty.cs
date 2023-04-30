using Domain.Abstracts;

namespace Domain.Entity
{
    public class ScheduledDuty : BaseEntity
    {
        public ScheduledDuty(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public Duty Duty { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DueOn { get; set; }
        public decimal ReminderDays { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }
    }
}
