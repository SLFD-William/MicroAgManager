using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockFeedDistribution : BaseEntity
    {
        public LivestockFeedDistribution(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockFeed Feed { get; set; }
        public decimal Quantity { get; set; }
        public bool? Discarded { get; set; }
        public string Note { get; set; }
        public DateTime DatePerformed { get; set; }
    }
}
