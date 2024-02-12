using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IMilestone
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Description { get; set; }
        
        string Name { get; set; }
        string RecipientType { get; set; }
        long RecipientTypeId { get; set; }
        bool SystemRequired { get; set; }

       ICollection<IDuty>? Duties { get; set; }
       ICollection<IEvent>? Events { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    [Index(nameof(RecipientType), nameof(RecipientTypeId))]
    public class Milestone : BaseEntity, IMilestone
    {
        public Milestone(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required] public bool SystemRequired { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();
        [NotMapped] ICollection<IDuty>? IMilestone.Duties { get => Duties as ICollection<IDuty>; set => Duties = value as ICollection<Duty> ?? new List<Duty>(); }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        [NotMapped] ICollection<IEvent>? IMilestone.Events { get => Events as ICollection<IEvent>; set => Events = value as ICollection<Event> ?? new List<Event>(); }
    }
}
