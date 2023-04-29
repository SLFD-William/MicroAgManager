using Domain.Abstracts;
using System.ComponentModel;

namespace Domain.Entity
{
    public class LivestockFeedAnalysisResult : BaseEntity
    {
        public LivestockFeedAnalysisResult(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public LivestockFeedAnalysis Analysis { get; set; }
        public LivestockFeedAnalysisParameter Parameter { get; set; }
        [Description("As Fed")]
        public decimal AsFed { get; set; }
        public decimal Dry { get; set; }

    }
}
