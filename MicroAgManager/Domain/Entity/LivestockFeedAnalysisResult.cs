using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class LivestockFeedAnalysisResult : BaseEntity
    {
        public LivestockFeedAnalysisResult(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Analysis")] public long  AnalysisId { get; set; }
        [Required][ForeignKey("Parameter")] public long ParameterId { get; set; }
        [Precision(18, 2)][Description("As Fed")] public decimal AsFed { get; set; }
        [Precision(18, 2)] public decimal Dry { get; set; }
        public virtual LivestockFeedAnalysis Analysis { get; set; }
        public virtual LivestockFeedAnalysisParameter Parameter { get; set; }
    }
}
