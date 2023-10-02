using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Duty : BaseEntity
    {
        public Duty(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        
        [MaxLength(1)] public string? Gender { get; set; }
        [Required] public bool SystemRequired { get; set; }
        [Required] public int DaysDue { get; set; }
        [Required] public long CommandId { get; set; }
        [Required][MaxLength(20)] public string Command { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required][MaxLength(20)] public string Relationship { get; set; }
        [Required][MaxLength(40)]public string Name { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public virtual ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
    }
}
