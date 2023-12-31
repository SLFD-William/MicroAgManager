namespace Domain.ValueObjects
{
    public class DutyChore : ValueObject
    {
        public DutyChore(long dutyId, long choreId, DateTime modifiedOn)
        {
            DutiesId = dutyId;
            ChoresId = choreId;
            ModifiedOn = modifiedOn;
        }

        public long DutiesId { get; set; }
        public long ChoresId { get; set; }
        public DateTime ModifiedOn { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DutiesId;
            yield return ChoresId;
            yield return ModifiedOn;
        }
    }
}
