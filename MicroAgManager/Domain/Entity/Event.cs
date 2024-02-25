using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IEvent
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Color { get; set; }
        DateTime? EndDate { get; set; }
        string Name { get; set; }
        DateTime StartDate { get; set; }
       ICollection<IDuty>? Duties { get; set; }
       ICollection<IMilestone>? Milestones { get; set; }
        ICollection<IScheduledDuty>? ScheduledDuties { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Event : BaseEntity, IEvent
    {
        public Event(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string Color { get; set; } = "transparent";
        [Required] public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();
         ICollection<IDuty>? IEvent.Duties { get => Duties as ICollection<IDuty>; set => Duties = value as ICollection<Duty> ??  new List<Duty>(); }


        public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
         ICollection<IMilestone>? IEvent.Milestones { get => Milestones as ICollection<IMilestone>; set => Milestones = value as ICollection<Milestone> ?? new List<Milestone>(); }


        public virtual ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
         ICollection<IScheduledDuty>? IEvent.ScheduledDuties { get => Milestones as ICollection<IScheduledDuty>; set => ScheduledDuties = value as ICollection<ScheduledDuty> ?? new List<ScheduledDuty>(); }

    }
}
