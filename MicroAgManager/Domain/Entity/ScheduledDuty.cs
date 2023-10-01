using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class ScheduledDuty : BaseEntity
    {
        public ScheduledDuty(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Duty")]public long DutyId { get; set; }

        public long? RecordId { get; set; }
        [Required] public string Record { get; set; }
        [Required] public long RecipientId { get; set; }
        [Required] public string Recipient { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DueOn { get; set; }
        [Precision(18,3)]public decimal ReminderDays { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }
        public virtual Duty Duty { get; set; }

    }
}
