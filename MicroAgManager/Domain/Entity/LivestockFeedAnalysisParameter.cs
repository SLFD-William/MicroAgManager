using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockFeedAnalysisParameter : BaseEntity
    {
        public LivestockFeedAnalysisParameter(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string Parameter { get; set; }
        public string SubParameter { get; set; }
        public string Unit { get; set; }
        public string Method { get; set; }
        public int ReportOrder { get; set; }
        public ICollection<LivestockFeedAnalysisResult> Results { get; private set; } = new List<LivestockFeedAnalysisResult>();

    }
}
