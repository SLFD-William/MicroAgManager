using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class LivestockFeed : BaseEntity
    {
        public LivestockFeed(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required] [ForeignKey("LivestockType")] public long LivestockTypeId { get; set; }
        
        [Required][MaxLength(255)]public string Name { get; set; }
        [Required][MaxLength(255)] public string Source { get; set; }
        public int? Cutting { get; set; } //Hay Only
        [Required] public bool Active { get; set; }
        [Required] [Precision(18,3)] public decimal Quantity { get; set; }
        [Required][MaxLength(20)] public string QuantityUnit { get; set; }
        [Required][Precision(18, 3)] public decimal QuantityWarning { get; set; }
        [Required][MaxLength(20)] public string FeedType { get; set; }
        [Required][MaxLength(20)] public string Distribution { get; set; }
        public virtual LivestockType LivestockType { get; set; }
        public virtual ICollection<LivestockFeedServing> Servings { get; private set; } = new List<LivestockFeedServing>();
        public virtual ICollection<LivestockFeedDistribution> Distributions { get; private set; } = new List<LivestockFeedDistribution>();
        public virtual ICollection<LivestockFeedAnalysis> Analyses { get; private set; } = new List<LivestockFeedAnalysis>();
    }
}
