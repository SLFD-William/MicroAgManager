using Domain.Entity;

namespace Domain.ValueObjects
{
    public class LandPlotLivestock : ValueObject
    {
        public LandPlotLivestock(long locationsId, long livestocksId)
        {
            LocationsId = locationsId;
            LivestocksId = livestocksId;
        }

        public long LocationsId { get; set; }
        public long LivestocksId { get; set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LocationsId;
            yield return LivestocksId;
        }
    }
}
