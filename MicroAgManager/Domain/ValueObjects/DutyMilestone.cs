using Domain.Entity;

namespace Domain.ValueObjects
{
    public class DutyMilestone : ValueObject
    {
        public DutyMilestone(long dutiesId, long milestonesId)
        {
            DutiesId = dutiesId;
            MilestonesId = milestonesId;
        }

        public long DutiesId { get; set; }
        public long MilestonesId { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DutiesId;
            yield return MilestonesId;
        }
    }
}
