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
    public class ScheduledDutyModel : BaseModel
    {
        private const string NO_RECIPIENT = "No Recipient";
        private const string NO_RECORD = "No Record";

        [Required][ForeignKey(nameof(DutyModel))] public long DutyId { get; set; }
        public string DutyName { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DueOn { get; set; }
        public long? RecordId { get; set; }
        [Required] public string Record { get; set; }
        public string RecordName { get; set; }
        [Required] public long RecipientId { get; set; }
        [Required] public string Recipient { get; set; }
        public string RecipientName { get; set; }
        [Precision(18,3)] public decimal ReminderDays { get; set; }
        public DateTime? CompletedOn { get; set; }
        public Guid? CompletedBy { get; set; }

        public static ScheduledDutyModel? Create(ScheduledDuty? duty,IMicroAgManagementDbContext db)
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
                RecordName = getRecord(duty, db),
                RecordId = duty.RecordId,
                Recipient = duty.Recipient,
                RecipientName = getRecipient(duty, db),
                RecipientId = duty.RecipientId,
                DutyName = string.IsNullOrEmpty(duty.Duty?.Name) ? db.Duties.Find(duty.DutyId)?.Name ?? "Unknown" : duty.Duty.Name,
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
        private static string getRecipient(ScheduledDuty duty,IMicroAgManagementDbContext db)
        {
            switch (duty.Recipient)
            {
                case "Livestock":
                    {
                        var livestock = db.Livestocks.Find(duty.RecipientId);
                        if (livestock is null) return NO_RECIPIENT;
                        var breed = db.LivestockBreeds.Find(livestock.LivestockBreedId);
                        return $"{breed?.EmojiChar} {breed?.Name} {livestock.Name}".Trim();
                    }
            }
            return NO_RECIPIENT;
        }
        private static string getRecord(ScheduledDuty duty, IMicroAgManagementDbContext db)
        {
            switch (duty.Record)
            {
                case "BreedingRecord":
                    {
                        var breedingRecord = db.BreedingRecords.Find(duty.RecordId);
                        return (breedingRecord is not null) ? $"Breeding Record: {breedingRecord.Id}".Trim() : NO_RECORD;
                    }
                case "Measurement":
                    {
                        var measurement = db.Measurements.Find(duty.RecordId);
                        return (measurement is not null) ? $"Measurement: {measurement.Id}".Trim() : NO_RECORD;
                    }
                case "TreatmentRecord":
                    {
                        var treatmentRecord = db.TreatmentRecords.Find(duty.RecordId);
                        return (treatmentRecord is not null) ? $"Treatment Record: {treatmentRecord.Id}".Trim() : NO_RECORD;
                    }
            }
            return NO_RECORD;
        }
    }
}
