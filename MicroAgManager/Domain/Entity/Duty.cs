using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IDuty
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Command { get; set; }
        long CommandId { get; set; }
        int DaysDue { get; set; }
        string? Gender { get; set; }
        string Name { get; set; }
        string? ProcedureLink { get; set; }
        string RecipientType { get; set; }
        long RecipientTypeId { get; set; }
        string Relationship { get; set; }
        bool SystemRequired { get; set; }
        ICollection<IChore>? Chores { get; set; }
       ICollection<IEvent>? Events { get; set; }
         ICollection<IMilestone>? Milestones { get; set; }
       ICollection<IScheduledDuty>? ScheduledDuties { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Duty : BaseEntity, IDuty
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
        [Required][MaxLength(40)] public string Name { get; set; }
        [MaxLength(255)] public string? ProcedureLink { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public virtual ICollection<ScheduledDuty> ScheduledDuties { get; set; } = new List<ScheduledDuty>();
        public virtual ICollection<Chore> Chores { get; set; } = new List<Chore>();
         ICollection<IChore>? IDuty.Chores { get => Chores as ICollection<IChore>; set => Chores = value as ICollection<Chore> ??  new List<Chore>(); }
         ICollection<IEvent>? IDuty.Events { get => Events as ICollection<IEvent>; set => Events = value as ICollection<Event> ?? new List<Event>(); }
         ICollection<IMilestone>? IDuty.Milestones { get => Milestones as ICollection<IMilestone >; set => Milestones = value as ICollection<Milestone> ?? new List<Milestone>(); }
         ICollection<IScheduledDuty>? IDuty.ScheduledDuties { get => ScheduledDuties as ICollection<IScheduledDuty>; set => ScheduledDuties = value as ICollection<ScheduledDuty> ?? new List<ScheduledDuty>(); }
    }
}
