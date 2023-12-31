using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Event : BaseEntity
    {
        public Event(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string Color { get; set; } = "transparent";
        [Required] public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();
        public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public virtual ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
        
    }
}
