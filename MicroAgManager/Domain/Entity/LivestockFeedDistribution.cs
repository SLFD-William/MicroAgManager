using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestockFeedDistribution
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        DateTime DatePerformed { get; set; }
        bool Discarded { get; set; }
        long LivestockFeedId { get; set; }
        string Note { get; set; }
        decimal Quantity { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockFeedDistribution : BaseEntity, ILivestockFeedDistribution
    {
        public LivestockFeedDistribution(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Feed")] public long LivestockFeedId { get; set; }
        [Precision(18, 3)] public decimal Quantity { get; set; }
        [Required] public bool Discarded { get; set; }
        [MaxLength(50)] public string Note { get; set; }
        [Required] public DateTime DatePerformed { get; set; }
        public virtual LivestockFeed Feed { get; set; }
    }
}
