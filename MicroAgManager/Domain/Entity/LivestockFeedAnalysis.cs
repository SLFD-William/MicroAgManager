using Domain.Abstracts;

namespace Domain.Entity
{
    public class LivestockFeedAnalysis : BaseEntity
    {
        public LivestockFeedAnalysis(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string LabNumber { get; set; }
        public LivestockFeed Feed { get; set; }
        public string TestCode { get; set; }
        public DateTime? DateSampled { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DatePrinted { get; set; }
        public ICollection< LivestockFeedAnalysisResult> Results { get; set; } = new List<LivestockFeedAnalysisResult>();

    }
}
