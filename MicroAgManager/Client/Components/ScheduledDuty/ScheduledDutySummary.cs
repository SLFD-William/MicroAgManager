using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.ScheduledDuty
{
    public class ScheduledDutySummary : ValueObject
    {
        private const string NO_RECIPIENT = "No Recipient";
        private const string NO_RECORD = "No Record";
        private ScheduledDutyModel _scheduledDutyModel;
        public ScheduledDutySummary(ScheduledDutyModel scheduledDutyModel, FrontEndDbContext context)
        {
            _scheduledDutyModel = context.ScheduledDuties.Find(scheduledDutyModel.Id);
            Duty=context.Duties.Find(_scheduledDutyModel?.DutyId)?.Name ?? string.Empty;
            Recipient=getRecipient(_scheduledDutyModel, context);
            Record=getRecord(_scheduledDutyModel, context);
        }

        private string getRecipient(ScheduledDutyModel scheduledDutyModel, FrontEndDbContext context)
        {
            switch (scheduledDutyModel.Recipient)
            {
                case "Livestock":
                    {
                        var livestock = context.Livestocks.Find(scheduledDutyModel.RecipientId);
                        if(livestock is null) return NO_RECIPIENT;
                        var breed = context.LivestockBreeds.Find(livestock.LivestockBreedId);
                        return $"{breed?.EmojiChar} {breed?.Name} {livestock.Name}".Trim();
                    }
            }
            return NO_RECIPIENT;
        }
        private string getRecord(ScheduledDutyModel scheduledDutyModel, FrontEndDbContext context)
        {
            switch (scheduledDutyModel.Record)
            {
                case "BreedingRecord":
                    {
                        var breedingRecord = context.BreedingRecords.Find(scheduledDutyModel.RecordId);
                        return (breedingRecord is not null) ? $"Breeding Record: {breedingRecord.Id}".Trim() : NO_RECORD;
                    }
            }
            return NO_RECORD;
        }
        public long Id => _scheduledDutyModel.Id;
        public string Duty { get; private set; }
        public DateTime DueOn => _scheduledDutyModel.DueOn;
        public string Recipient { get; private set; }
        public string Record { get; private set; }
        public bool Dismissed => _scheduledDutyModel.Dismissed;
        public DateTime? Completed => _scheduledDutyModel.CompletedOn;


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _scheduledDutyModel;
        }
    }
}
