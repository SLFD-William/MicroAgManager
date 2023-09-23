using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockBreedModel : BaseModel
    {
        [Required]
        [ForeignKey(nameof(LivestockAnimalModel))]
        public long LivestockAnimalId { get; set; }
        [Required] [MaxLength(40)]public string Name { get; set; }
        [MaxLength(2)] public string EmojiChar { get; set; }
        [Required] public int GestationPeriod { get; set; }
        [Required] public int HeatPeriod { get; set; }
        public virtual ICollection<LivestockModel> Livestocks { get; set; } = new List<LivestockModel>();
        public static LivestockBreedModel Create(LivestockBreed livestockBreed)
        {
            var model = PopulateBaseModel(livestockBreed, new LivestockBreedModel
            {
                LivestockAnimalId=livestockBreed.LivestockAnimalId,
                Name = livestockBreed.Name,
                EmojiChar = livestockBreed.EmojiChar,
                GestationPeriod = livestockBreed.GestationPeriod,
                HeatPeriod = livestockBreed.HeatPeriod,
                Livestocks=livestockBreed.Livestocks.Select(LivestockModel.Create).ToList() ?? new List<LivestockModel>()
        }) as LivestockBreedModel;
            return model;
        }
        public LivestockBreed MapToEntity(LivestockBreed entity)
        {
            entity.LivestockAnimalId = LivestockAnimalId;
            entity.EmojiChar = EmojiChar;
            entity.GestationPeriod = GestationPeriod;  
            entity.HeatPeriod = HeatPeriod;
            entity.Name = Name;
            entity.ModifiedOn = DateTime.UtcNow;
            if (entity.Livestocks.Any())
                foreach (var livestock in entity.Livestocks)
                    Livestocks.FirstOrDefault(p => p?.Id == livestock.Id)?.MapToEntity(livestock);
            return entity;
        }
    }
}
