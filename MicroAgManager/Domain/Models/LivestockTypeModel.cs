using Domain.Abstracts;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class LivestockTypeModel : BaseModel
    {
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string GroupName { get; set; }
        [Required][MaxLength(40)] public string ParentMaleName { get; set; }
        [Required][MaxLength(40)] public string ParentFemaleName { get; set; }
        [Required][MaxLength(40)] public string DefaultStatus { get; set; }
        [Required][MaxLength(40)] public string Care { get; set; }
        public virtual ICollection<LivestockBreedModel?> Breeds { get; set; } =new List<LivestockBreedModel?>();
        public virtual ICollection<LivestockStatusModel?> Statuses { get; set; } = new List<LivestockStatusModel?>();
        public virtual ICollection<LivestockFeedModel?> Feeds { get; set; } = new List<LivestockFeedModel?>();

        public static LivestockTypeModel? Create(LivestockType livestockType)
        {
            var model = PopulateBaseModel(livestockType, new LivestockTypeModel
            {
                 Care=livestockType.Care,
                 GroupName=livestockType.GroupName,
                 DefaultStatus=livestockType.DefaultStatus,
                 Name=livestockType.Name,
                 ParentFemaleName = livestockType.ParentFemaleName,
                 ParentMaleName = livestockType.ParentMaleName,
                 Breeds=livestockType.Breeds.Select(LivestockBreedModel.Create).ToList() ?? new List<LivestockBreedModel?>(),
                 Statuses = livestockType.Statuses.Select(LivestockStatusModel.Create).ToList() ?? new List<LivestockStatusModel?>(),
                 Feeds = livestockType.Feeds.Select(LivestockFeedModel.Create).ToList() ?? new List<LivestockFeedModel?>()

            }) as LivestockTypeModel;
            return model;
        }

        public LivestockType MapToEntity(LivestockType entity)
        {
            entity.ParentMaleName=ParentMaleName;
            entity.ParentFemaleName=ParentFemaleName;
            entity.Care=Care;
            entity.DefaultStatus=DefaultStatus;
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
            return entity;
        }
    }
}
