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

        public LivestockAnimal MapToEntity(LivestockAnimal entity)
        {
            entity.ParentMaleName=ParentMaleName;
            entity.ParentFemaleName=ParentFemaleName;
            entity.Care=Care;
            entity.GroupName=GroupName;
            entity.Name=Name;
            if (entity.Breeds?.Any() ?? false)
                foreach (var breed in entity.Breeds)
                    Breeds?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            if (entity.Statuses?.Any() ?? false)
                foreach (var breed in entity.Statuses)
                    Statuses?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            if (entity.Feeds?.Any() ?? false)
                foreach (var breed in entity.Feeds)
                    Feeds?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
