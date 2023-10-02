using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Milestone : BaseEntity
    {
        public Milestone(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] [MaxLength(40)]public string Name { get; set; }
        [Required]public bool SystemRequired { get; set; }
        [Required][MaxLength(255)] public string Description { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Duty> Duties { get; set; } = new List<Duty>();

    }
}
