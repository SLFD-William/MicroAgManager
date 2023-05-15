using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class LivestockFeedAnalysis : BaseEntity
    {
        public LivestockFeedAnalysis(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("LivestockFeed")]public long LivestockFeedId { get; set; }
        [MaxLength(40)]public string LabNumber { get; set; }

        [MaxLength(40)] public string TestCode { get; set; }
        public DateTime? DateSampled { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DatePrinted { get; set; }
        public virtual ICollection< LivestockFeedAnalysisResult> Results { get; set; } = new List<LivestockFeedAnalysisResult>();
        public virtual LivestockFeed LivestockFeed { get; set; }

    }
}
