using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class LivestockFeedServing : BaseEntity
    {
        public LivestockFeedServing(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Feed")]public long LivestockFeedId { get; set; }

        //add on delete no action and on update no action

        [Required]
        [ForeignKey("Status")]
        
        public long LivestockStatusId { get; set; }
        [Required]public string ServingFrequency { get; set; }
        [Precision(18,3)]public decimal Serving { get; set; }
        public virtual LivestockFeed Feed { get; set; }

        public virtual LivestockStatus Status { get; set; }
    }

}
