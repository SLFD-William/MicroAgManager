using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class LivestockAnimalModel : BaseModel,ILivestockAnimal
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string GroupName { get; set; }
        [Required][MaxLength(40)] public string ParentMaleName { get; set; }
        [Required][MaxLength(40)] public string ParentFemaleName { get; set; }
        [Required][MaxLength(40)] public string Care { get; set; } = LivestockCareConstants.Individual;
        public virtual ICollection<LivestockBreedModel?> Breeds { get; set; } =new List<LivestockBreedModel?>();
        public virtual ICollection<LivestockStatusModel?> Statuses { get; set; } = new List<LivestockStatusModel?>();
        public virtual ICollection<LivestockFeedModel?> Feeds { get; set; } = new List<LivestockFeedModel?>();
        [NotMapped] DateTime ILivestockAnimal.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value == EntityModifiedOn ? EntityModifiedOn : EntityModifiedOn; }
        [NotMapped] ICollection<ILivestockBreed>? ILivestockAnimal.Breeds { get => Breeds as ICollection<ILivestockBreed>; set => Breeds = value as ICollection<LivestockBreedModel?> ?? new List<LivestockBreedModel?>(); }
        [NotMapped] ICollection<ILivestockFeed>? ILivestockAnimal.Feeds { get => Feeds as ICollection<ILivestockFeed>; set => Feeds = value as ICollection<LivestockFeedModel?> ?? new List<LivestockFeedModel?>(); }
        [NotMapped] ICollection<ILivestockStatus>? ILivestockAnimal.Statuses { get => Statuses as ICollection<ILivestockStatus>; set => Statuses = value as ICollection<LivestockStatusModel?> ?? new List<LivestockStatusModel?>(); }

        public static LivestockAnimalModel? Create(LivestockAnimal LivestockAnimal)
        {
            var model = PopulateBaseModel(LivestockAnimal, new LivestockAnimalModel
            {
                 Care=LivestockAnimal.Care,
                 GroupName=LivestockAnimal.GroupName,
                 Name=LivestockAnimal.Name,
                 ParentFemaleName = LivestockAnimal.ParentFemaleName,
                 ParentMaleName = LivestockAnimal.ParentMaleName,
                 Breeds=LivestockAnimal.Breeds.Select(LivestockBreedModel.Create).ToList() ?? new List<LivestockBreedModel?>(),
                 Statuses = LivestockAnimal.Statuses.Select(LivestockStatusModel.Create).ToList() ?? new List<LivestockStatusModel?>(),
                 Feeds = LivestockAnimal.Feeds.Select(LivestockFeedModel.Create).ToList() ?? new List<LivestockFeedModel?>()

            }) as LivestockAnimalModel;
            return model;
        }
        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity== null ||entity is not LivestockAnimal) return null;
            ((LivestockAnimal)entity).ParentMaleName = ParentMaleName;
            ((LivestockAnimal)entity).ParentFemaleName = ParentFemaleName;
            ((LivestockAnimal)entity).Care = Care;
            ((LivestockAnimal)entity).GroupName = GroupName;
            ((LivestockAnimal)entity).Name = Name;
            ((LivestockAnimal)entity).ModifiedOn = DateTime.UtcNow;
            return entity;
        }

        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockAnimalModel) return null;
            ((LivestockAnimalModel)entity).ParentMaleName = ParentMaleName;
            ((LivestockAnimalModel)entity).ParentFemaleName = ParentFemaleName;
            ((LivestockAnimalModel)entity).Care = Care;
            ((LivestockAnimalModel)entity).GroupName = GroupName;
            ((LivestockAnimalModel)entity).Name = Name;
            ((LivestockAnimalModel)entity).EntityModifiedOn = EntityModifiedOn;
            return entity;
        }
    }
}
