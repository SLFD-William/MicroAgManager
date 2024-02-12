using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestockBreed
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string EmojiChar { get; set; }
        int GestationPeriod { get; set; }
        int HeatPeriod { get; set; }
        long LivestockAnimalId { get; set; }
       ICollection<ILivestock>? Livestocks { get; set; }
        string Name { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class LivestockBreed : BaseEntity, ILivestockBreed
    {
        public LivestockBreed(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("LivestockAnimal")] public long LivestockAnimalId { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [MaxLength(2)] public string EmojiChar { get; set; }
        [Required] public int GestationPeriod { get; set; }
        [Required] public int HeatPeriod { get; set; }
        public virtual ICollection<Livestock> Livestocks { get; set; } = new List<Livestock>();
        public virtual LivestockAnimal LivestockAnimal { get; set; }
       [NotMapped] ICollection<ILivestock>? ILivestockBreed.Livestocks { get => Livestocks as ICollection<ILivestock>; set => Livestocks = value as ICollection<Livestock> ?? new List<Livestock>(); }

    }
}
