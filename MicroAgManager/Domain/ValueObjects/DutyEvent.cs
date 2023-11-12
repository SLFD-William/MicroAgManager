namespace Domain.ValueObjects
{
    public class DutyEvent : ValueObject
    {
        public DutyEvent(long dutiesId, long eventId, DateTime modifiedOn)
        {
            DutiesId = dutiesId;
            EventsId = eventId;
            ModifiedOn = modifiedOn;
        }

        public long DutiesId { get; set; }
        public long EventsId { get; set; }
        public DateTime ModifiedOn { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DutiesId;
            yield return EventsId;
            yield return ModifiedOn;
        }
    }
}
