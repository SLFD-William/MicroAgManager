namespace Domain.ValueObjects
{
    public class DutyMilestone : ValueObject
    {
        public DutyMilestone(long dutiesId, long milestonesId, DateTime modifiedOn)
        {
            DutiesId = dutiesId;
            MilestonesId = milestonesId;
            ModifiedOn = modifiedOn;
        }

        public long DutiesId { get; set; }
        public long MilestonesId { get; set; }
        public DateTime ModifiedOn { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DutiesId;
            yield return MilestonesId;
            yield return ModifiedOn;
        }
    }
}
