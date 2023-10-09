using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class LivestockAnimalModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string GroupName { get; set; }
        [Required][MaxLength(40)] public string ParentMaleName { get; set; }
        [Required][MaxLength(40)] public string ParentFemaleName { get; set; }
        [Required][MaxLength(40)] public string Care { get; set; } = LivestockCareConstants.Individual;
        public virtual ICollection<LivestockBreedModel?> Breeds { get; set; } =new List<LivestockBreedModel?>();
        public virtual ICollection<LivestockStatusModel?> Statuses { get; set; } = new List<LivestockStatusModel?>();
        public virtual ICollection<LivestockFeedModel?> Feeds { get; set; } = new List<LivestockFeedModel?>();

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
            if (((LivestockAnimal)entity).Breeds?.Any() ?? false)
                foreach (var breed in ((LivestockAnimal)entity).Breeds)
                    Breeds?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            if (((LivestockAnimal)entity).Statuses?.Any() ?? false)
                foreach (var breed in ((LivestockAnimal)entity).Statuses)
                    Statuses?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            if (((LivestockAnimal)entity).Feeds?.Any() ?? false)
                foreach (var breed in ((LivestockAnimal)entity).Feeds)
                    Feeds?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
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
            if (((LivestockAnimalModel)entity).Breeds?.Any() ?? false)
                foreach (var breed in ((LivestockAnimalModel)entity).Breeds)
                    Breeds?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            if (((LivestockAnimalModel)entity).Statuses?.Any() ?? false)
                foreach (var breed in ((LivestockAnimalModel)entity).Statuses)
                    Statuses?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            if (((LivestockAnimalModel)entity).Feeds?.Any() ?? false)
                foreach (var breed in ((LivestockAnimalModel)entity).Feeds)
                    Feeds?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            return entity;
        }
    }
}
