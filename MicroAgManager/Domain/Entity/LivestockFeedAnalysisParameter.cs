using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public interface ILivestockFeedAnalysisParameter
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string Method { get; set; }
        string Parameter { get; set; }
        int ReportOrder { get; set; }
     ICollection<ILivestockFeedAnalysisResult> Results { get; }
        string SubParameter { get; set; }
        string Unit { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockFeedAnalysisParameter : BaseEntity, ILivestockFeedAnalysisParameter
    {
        public LivestockFeedAnalysisParameter(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(50)] public string Parameter { get; set; }
        [Required][MaxLength(50)] public string SubParameter { get; set; }
        [Required][MaxLength(50)] public string Unit { get; set; }
        [Required][MaxLength(50)] public string Method { get; set; }
        [Required] public int ReportOrder { get; set; }
        public virtual ICollection<LivestockFeedAnalysisResult> Results { get; private set; } = new List<LivestockFeedAnalysisResult>();

        ICollection<ILivestockFeedAnalysisResult> ILivestockFeedAnalysisParameter.Results => throw new NotImplementedException();
    }
}
