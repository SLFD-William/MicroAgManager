using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    [Index(nameof(Name), IsUnique =true)]
    public class LivestockAnimal : BaseEntity
    {
        public LivestockAnimal(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required] [MaxLength(40)]public string Name { get; set; }
        [Required][MaxLength(40)] public string GroupName { get; set; }
        [Required][MaxLength(40)] public string ParentMaleName { get; set; }
        [Required][MaxLength(40)] public string ParentFemaleName { get; set; }
        [Required][MaxLength(40)] public string Care { get; set; }
        public virtual ICollection<LivestockBreed> Breeds { get; set; } = new List<LivestockBreed>();
        public virtual ICollection<LivestockStatus> Statuses { get; set; } = new List<LivestockStatus>();
        public virtual ICollection<LivestockFeed> Feeds { get; set; } = new List<LivestockFeed>();
    }
}
