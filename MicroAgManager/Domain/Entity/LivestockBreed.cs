using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockBreed : BaseEntity
    {
        public LivestockBreed(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("LivestockAnimal")] public long LivestockAnimalId { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [MaxLength(2)]public string EmojiChar { get; set; }
        [Required] public int GestationPeriod { get; set; }
        [Required] public int HeatPeriod { get; set; }
        public virtual ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();
        public virtual LivestockAnimal LivestockAnimal { get; set; }
    }
}
