using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockFeedServing : BaseEntity
    {
        public LivestockFeedServing(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockFeed Feed { get; set; }
        public LivestockStatus Status { get; set; }
        public string ServingFrequency { get; set; }
        public decimal Serving { get; set; }

    }

}
