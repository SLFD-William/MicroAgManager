using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Milestone : BaseEntity
    {
        public Milestone(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [ForeignKey("LivestockType")] public long? LivestockTypeId { get; set; }
        [Required] [MaxLength(40)]public string Subcategory { get; set; }
        [Required]public bool SystemRequired { get; set; }
        public virtual LivestockType? LivestockType { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();

    }
}
