using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestockFeed
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        bool Active { get; set; }
        ICollection<ILivestockFeedAnalysis>? Analyses { get; }
        int? Cutting { get; set; }
        string Distribution { get; set; }
       ICollection<ILivestockFeedDistribution>? Distributions { get; }
        string FeedType { get; set; }
        long LivestockAnimalId { get; set; }
        string Name { get; set; }
        decimal Quantity { get; set; }
        IUnit QuantityUnit { get; set; }
        decimal QuantityWarning { get; set; }
       ICollection<ILivestockFeedServing>? Servings { get; }
        string Source { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockFeed : BaseEntity, ILivestockFeed
    {
        public LivestockFeed(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("LivestockAnimal")] public long LivestockAnimalId { get; set; }

        [Required][MaxLength(255)] public string Name { get; set; }
        [Required][MaxLength(255)] public string Source { get; set; }
        public int? Cutting { get; set; } //Hay Only
        [Required] public bool Active { get; set; }
        [Required][Precision(18, 3)] public decimal Quantity { get; set; }
        [Required][MaxLength(20)] public string QuantityUnit { get; set; }
        [Required][Precision(18, 3)] public decimal QuantityWarning { get; set; }
        [Required][MaxLength(20)] public string FeedType { get; set; }
        [Required][MaxLength(20)] public string Distribution { get; set; }
        public virtual LivestockAnimal LivestockAnimal { get; set; }
        public virtual ICollection<LivestockFeedServing> Servings { get; private set; } = new List<LivestockFeedServing>();
        public virtual ICollection<LivestockFeedDistribution> Distributions { get; private set; } = new List<LivestockFeedDistribution>();
        public virtual ICollection<LivestockFeedAnalysis> Analyses { get; private set; } = new List<LivestockFeedAnalysis>();

        ICollection<ILivestockFeedAnalysis>? ILivestockFeed.Analyses => Analyses as ICollection<ILivestockFeedAnalysis>;

        ICollection<ILivestockFeedDistribution> ILivestockFeed.Distributions => throw new NotImplementedException();

        IUnit ILivestockFeed.QuantityUnit { get; set; }

        ICollection<ILivestockFeedServing> ILivestockFeed.Servings => throw new NotImplementedException();
    }
}
