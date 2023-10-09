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
        public override BaseModel Map(BaseModel duty)
        {
            if (duty == null || duty is not ScheduledDutyModel) return null;
            ((ScheduledDutyModel)duty).CompletedBy = CompletedBy;
            ((ScheduledDutyModel)duty).CompletedOn = CompletedOn;
            ((ScheduledDutyModel)duty).Dismissed = Dismissed;
            ((ScheduledDutyModel)duty).DueOn = DueOn;
            ((ScheduledDutyModel)duty).ReminderDays = ReminderDays;
            ((ScheduledDutyModel)duty).DutyId = DutyId;
            ((ScheduledDutyModel)duty).Recipient = Recipient;
            ((ScheduledDutyModel)duty).RecipientId = RecipientId;
            ((ScheduledDutyModel)duty).Record = Record;
            ((ScheduledDutyModel)duty).RecordId = RecordId;
            return duty;
        }

        public override BaseEntity Map(BaseEntity duty)
        {
            if (duty == null || duty is not ScheduledDuty) return null;
            ((ScheduledDuty)duty).CompletedBy = CompletedBy;
            ((ScheduledDuty)duty).CompletedOn = CompletedOn;
            ((ScheduledDuty)duty).Dismissed = Dismissed;
            ((ScheduledDuty)duty).DueOn = DueOn;
            ((ScheduledDuty)duty).ReminderDays = ReminderDays;
            ((ScheduledDuty)duty).DutyId = DutyId;
            duty.ModifiedOn = DateTime.UtcNow;
            ((ScheduledDuty)duty).Recipient = Recipient;
            ((ScheduledDuty)duty).RecipientId = RecipientId;
            ((ScheduledDuty)duty).Record = Record;
            ((ScheduledDuty)duty).RecordId = RecordId;
            return duty;
        }
    }
}
