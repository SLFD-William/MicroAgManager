using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockStatus : BaseEntity
    {
        public LivestockStatus(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string Status { get; set; }
        public LivestockType LivestockType { get; set; }
        public string InMilk { get; set; }
        public string BeingManaged { get; set; }
        public string Sterile { get; set; }
        public string BottleFed { get; set; }
        public string ForSale { get; set; }
        public ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();
        public ICollection<LivestockFeedServing> FeedServings { get; set; } = new List<LivestockFeedServing>();
    }
}
