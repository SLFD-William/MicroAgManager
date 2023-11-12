namespace Domain.ValueObjects
{
    public class LandPlotLivestock : ValueObject
    {
        public LandPlotLivestock(long locationsId, long livestocksId, DateTime modifiedOn)
        {
            LocationsId = locationsId;
            LivestocksId = livestocksId;
            ModifiedOn = modifiedOn;
        }

        public long LocationsId { get; set; }
        public long LivestocksId { get; set; }
        public DateTime ModifiedOn { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LocationsId;
            yield return LivestocksId;
            yield return ModifiedOn;
        }
    }
}
