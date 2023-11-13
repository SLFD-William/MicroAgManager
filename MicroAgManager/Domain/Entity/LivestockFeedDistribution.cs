using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockFeedDistribution : BaseEntity
    {
        public LivestockFeedDistribution(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Feed")] public long LivestockFeedId { get; set; }
        [Precision(18,3)]public decimal Quantity { get; set; }
        [Required]public bool Discarded { get; set; }
        [MaxLength(50)]public string Note { get; set; }
        [Required]public DateTime DatePerformed { get; set; }
        public virtual LivestockFeed Feed { get; set; }
    }
}
