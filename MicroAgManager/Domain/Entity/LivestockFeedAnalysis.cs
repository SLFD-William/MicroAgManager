using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestockFeedAnalysis
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        DateTime? DatePrinted { get; set; }
        DateTime? DateReceived { get; set; }
        DateTime? DateReported { get; set; }
        DateTime? DateSampled { get; set; }
        string LabNumber { get; set; }
        long LivestockFeedId { get; set; }
       ICollection<ILivestockFeedAnalysisResult> Results { get; set; }
        string TestCode { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockFeedAnalysis : BaseEntity, ILivestockFeedAnalysis
    {
        public LivestockFeedAnalysis(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("LivestockFeed")] public long LivestockFeedId { get; set; }
        [MaxLength(40)] public string LabNumber { get; set; }

        [MaxLength(40)] public string TestCode { get; set; }
        public DateTime? DateSampled { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DatePrinted { get; set; }
        public virtual ICollection<LivestockFeedAnalysisResult> Results { get; set; } = new List<LivestockFeedAnalysisResult>();
        public virtual LivestockFeed LivestockFeed { get; set; }
        ICollection<ILivestockFeedAnalysisResult> ILivestockFeedAnalysis.Results { get; set; }
    }
}
