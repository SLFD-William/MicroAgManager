using Domain.Abstracts;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Index(nameof(CompletedOn))]
    [Index(nameof(DueOn))]
    [Index(nameof(Dismissed))]
    public class ScheduledDutyModel : BaseModel,IScheduledDuty
    {
        [Required][ForeignKey(nameof(Duty))] public long DutyId { get; set; }
        public virtual DutyModel Duty { get; set; }

        public bool Dismissed { get; set; }
        public DateTime DueOn { get; set; }
        public long? RecordId { get; set; }
        [Required] public string Record { get; set; }//TreatmentRecord, Registration, BreedingRecord etc.
        [Required] public long RecipientId { get; set; }
        [Required] public string Recipient { get; set; }//Livestock, etc.
        [Required] public long ScheduleSourceId { get; set; }
        [Required] public string ScheduleSource { get; set; } //Chore,Event,Milestone
        [Precision(18,3)] public decimal ReminderDays { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }
        [NotMapped] DateTime IScheduledDuty.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
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
                RecipientId = duty.RecipientId,
                ScheduleSourceId = duty.ScheduleSourceId,
                ScheduleSource = duty.ScheduleSource
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
            ((ScheduledDutyModel)duty).ScheduleSourceId = ScheduleSourceId;
            ((ScheduledDutyModel)duty).ScheduleSource = ScheduleSource;
            ((ScheduledDutyModel)duty).EntityModifiedOn = EntityModifiedOn;
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
            ((ScheduledDuty)duty).ScheduleSource= ScheduleSource;
            ((ScheduledDuty)duty).ScheduleSourceId = ScheduleSourceId;
            return duty;
        }

        public void PopulateDynamicRelations(IFrontEndDbContext db)
        {
            if (ScheduleSource == nameof(Chore))
                ScheduleSourceItem = db.Chores.Find(ScheduleSourceId)?.Name;
            if (Recipient == nameof(Livestock))
                RecipientItem = db.Livestocks.Find(RecipientId)?.Name;
        }
        [NotMapped] public string ScheduleSourceItem { get; private set; } = string.Empty;
        [NotMapped] public string RecipientItem { get; private set; } = string.Empty;
       
    }
}
