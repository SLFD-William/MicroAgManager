namespace Domain.ValueObjects
{
    public class DutyEvent : ValueObject
    {
        public DutyEvent(long dutiesId, long eventId)
        {
            DutiesId =  dutiesId;
            EventsId = eventId;
        }

        public long DutiesId { get; set; }
        public long EventsId { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DutiesId;
            yield return EventsId;
        }
    }
}
