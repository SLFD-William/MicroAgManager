using Domain.Entity;

namespace Domain.ValueObjects
{
    public class LivestockLivestockStatus : ValueObject
    {
        public LivestockLivestockStatus(long statusId, long livestockId)
        {
            StatusesId = statusId;
            LivestocksId = livestockId;
        }

        public long LivestocksId { get; set; }
        public long StatusesId { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StatusesId;
            yield return LivestocksId;
        }
    }
}