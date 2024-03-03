using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface IScheduledDuty
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        Guid? CompletedBy { get; set; }
        DateTime? CompletedOn { get; set; }
        bool Dismissed { get; set; }
        DateTime DueOn { get; set; }
        long DutyId { get; set; }
        string Recipient { get; set; }
        long RecipientId { get; set; }
        string Record { get; set; }
        long? RecordId { get; set; }
        decimal ReminderDays { get; set; }
        string ScheduleSource { get; set; }
        long ScheduleSourceId { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    [Index(nameof(Recipient), nameof(RecipientId))]
    public class ScheduledDuty : BaseEntity, IScheduledDuty,ICloneable
    {
        public ScheduledDuty(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey(nameof(Duty))] public long DutyId { get; set; }

        public long? RecordId { get; set; }
        [Required] public string Record { get; set; }//TreatmentRecord, Registration, BreedingRecord etc.
        [Required] public long RecipientId { get; set; }
        [Required] public string Recipient { get; set; }//Livestock, etc.
        [Required] public long ScheduleSourceId { get; set; }
        [Required] public string ScheduleSource { get; set; } //Chore,Event,Milestone
        public bool Dismissed { get; set; }
        public DateTime DueOn { get; set; }
        [Precision(18, 3)] public decimal ReminderDays { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }
        public virtual Duty Duty { get; set; }

        public object Clone() => new ScheduledDuty(this.CreatedBy, this.TenantId)
        {
            DutyId = this.DutyId,
            RecordId = this.RecordId,
            Record = this.Record,
            RecipientId = this.RecipientId,
            Recipient = this.Recipient,
            ScheduleSourceId = this.ScheduleSourceId,
            ScheduleSource = this.ScheduleSource,
            Dismissed = this.Dismissed,
            DueOn = this.DueOn,
            ReminderDays = this.ReminderDays,
            CompletedOn = this.CompletedOn,
            CompletedBy = this.CompletedBy,
        };
    }
}
