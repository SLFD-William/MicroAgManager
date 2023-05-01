using Domain.Abstracts;

namespace Domain.Entity
{
    public class Duty : BaseEntity
    {
        public Duty(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockType? LivestockType { get; set; }
        public string Name { get; set; }
        public int DaysDue { get; set; }
        public string DutyType { get; set; }
        public long DutyTypeId { get; set; }
        public string? Gender { get; set; }
        public string Relationship { get; set; }
        public bool SystemRequired { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
    }
}
