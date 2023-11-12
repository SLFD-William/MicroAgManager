namespace Domain.ValueObjects
{
    public class LivestockLivestockStatus : ValueObject
    {
        public LivestockLivestockStatus(long statusId, long livestockId, DateTime modifiedOn)
        {
            StatusesId = statusId;
            LivestocksId = livestockId;
            ModifiedOn = modifiedOn;
        }

        public long LivestocksId { get; set; }
        public long StatusesId { get; set; }
        public DateTime ModifiedOn { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StatusesId;
            yield return LivestocksId;
            yield return ModifiedOn;
        }
    }
}