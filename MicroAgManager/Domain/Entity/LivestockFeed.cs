using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockFeed : BaseEntity
    {
        public LivestockFeed(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockType LivestockType { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public int? Cutting { get; set; } //Hay Only
        public bool? Active { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public decimal QuantityWarning { get; set; }
        public string FeedType { get; set; }
        public string Distribution { get; set; }
        public ICollection<LivestockFeedServing> Servings { get; private set; } = new List<LivestockFeedServing>();
        public ICollection<LivestockFeedDistribution> Distributions { get; private set; } = new List<LivestockFeedDistribution>();
        public ICollection<LivestockFeedAnalysis> Analyses { get; private set; } = new List<LivestockFeedAnalysis>();
    }
}
