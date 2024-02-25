using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public interface ILivestockAnimal
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        ICollection<ILivestockBreed>? Breeds { get; set; }
        string Care { get; set; }
       ICollection<ILivestockFeed>? Feeds { get; set; }
        string GroupName { get; set; }
        string Name { get; set; }
        string ParentFemaleName { get; set; }
        string ParentMaleName { get; set; }
       ICollection<ILivestockStatus>? Statuses { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    [Index(nameof(Name), IsUnique = true)]
    public class LivestockAnimal : BaseEntity, ILivestockAnimal
    {
        public LivestockAnimal(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string GroupName { get; set; }
        [Required][MaxLength(40)] public string ParentMaleName { get; set; }
        [Required][MaxLength(40)] public string ParentFemaleName { get; set; }
        [Required][MaxLength(40)] public string Care { get; set; }
        public virtual ICollection<LivestockBreed> Breeds { get; set; } = new List<LivestockBreed>();
        public virtual ICollection<LivestockStatus> Statuses { get; set; } = new List<LivestockStatus>();
        public virtual ICollection<LivestockFeed> Feeds { get; set; } = new List<LivestockFeed>();
        ICollection<ILivestockBreed>? ILivestockAnimal.Breeds { get => Breeds as ICollection<ILivestockBreed>; set => Breeds=value as ICollection<LivestockBreed> ?? new List<LivestockBreed>(); }
        ICollection<ILivestockFeed>? ILivestockAnimal.Feeds { get => Feeds as ICollection<ILivestockFeed>; set => Feeds = value as ICollection<LivestockFeed> ?? new List<LivestockFeed>(); }
        ICollection<ILivestockStatus>? ILivestockAnimal.Statuses { get => Statuses as ICollection<ILivestockStatus>; set => Statuses = value as ICollection<LivestockStatus> ?? new List<LivestockStatus>(); }
    }
}
