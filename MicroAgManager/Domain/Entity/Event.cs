using Domain.Abstracts;

namespace Domain.Entity
{
    public class Event : BaseEntity
    {
        public Event(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string Name { get; set; }
        public string Color { get; set; }
        public ICollection<Duty> Duties { get; set; } = new List<Duty>();
        public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
