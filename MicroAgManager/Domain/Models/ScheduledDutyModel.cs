using Domain.Abstracts;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Index(nameof(CompletedOn))]
    [Index(nameof(DueOn))]
    [Index(nameof(Dismissed))]
    public class ScheduledDutyModel : BaseModel
    {
        [Required][ForeignKey(nameof(DutyModel))] public long DutyId { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DueOn { get; set; }
        public long? RecordId { get; set; }
        [Required] public string Record { get; set; }
        [Required] public long RecipientId { get; set; }
        [Required] public string Recipient { get; set; }
        [Precision(18,3)] public decimal ReminderDays { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }
        public static ScheduledDutyModel? Create(ScheduledDuty? duty)
        {
            if (duty == null) return null;
            var model = PopulateBaseModel(duty, new ScheduledDutyModel
            {
                CompletedBy = duty.CompletedBy,
                CompletedOn = duty.CompletedOn,
                Dismissed = duty.Dismissed,
                DueOn = duty.DueOn,
                ReminderDays = duty.ReminderDays,
                DutyId = duty.DutyId,
                Record = duty.Record,
                RecordId = duty.RecordId,
                Recipient = duty.Recipient,
                RecipientId = duty.RecipientId
            }) as ScheduledDutyModel;
            return model;
        }
        public ScheduledDuty MapToEntity(ScheduledDuty duty)
        {
            duty.CompletedBy = CompletedBy;
            duty.CompletedOn = CompletedOn;
            duty.Dismissed = Dismissed;
            duty.DueOn = DueOn;
            duty.ReminderDays = ReminderDays;
            duty.DutyId=DutyId;
            duty.ModifiedOn = DateTime.UtcNow;
            duty.Recipient = Recipient;
            duty.RecipientId = RecipientId;
            duty.Record = Record;
            duty.RecordId = RecordId;

            return duty;
        }


    }
}
