using Domain.Abstracts;

namespace Domain.Entity
{
    public class Milestone : BaseEntity
    {
        public Milestone(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string Subcategory { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Duty> Duties { get; set; } = new List<Duty>();
        public bool SystemRequired { get; set; }
    }
}
