using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LivestockBreedModel : BaseModel,ILivestockBreed
    {
        [Required]
        [ForeignKey(nameof(Animal))]
        public long LivestockAnimalId { get; set; }
        [Required] [MaxLength(40)]public string Name { get; set; }
        [MaxLength(2)] public string EmojiChar { get; set; }
        [Required] public int GestationPeriod { get; set; }
        [Required] public int HeatPeriod { get; set; }
        public virtual ICollection<LivestockModel> Livestocks { get; set; } = new List<LivestockModel>();
        
        public virtual LivestockAnimalModel Animal { get; set; }
        [NotMapped] DateTime ILivestockBreed.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
        [NotMapped] ICollection<ILivestock>? ILivestockBreed.Livestocks { get => Livestocks as ICollection<ILivestock>; set =>Livestocks=value as ICollection<LivestockModel> ?? new List<LivestockModel>(); }

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
           public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockBreedModel) return null;
            ((LivestockBreedModel)entity).LivestockAnimalId = LivestockAnimalId;
            ((LivestockBreedModel)entity).EmojiChar = EmojiChar;
            ((LivestockBreedModel)entity).GestationPeriod = GestationPeriod;
            ((LivestockBreedModel)entity).HeatPeriod = HeatPeriod;
            ((LivestockBreedModel)entity).Name = Name;
            ((LivestockBreedModel)entity).EntityModifiedOn = EntityModifiedOn;
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockBreed) return null;
            ((LivestockBreed)entity).LivestockAnimalId = LivestockAnimalId;
            ((LivestockBreed)entity).EmojiChar = EmojiChar;
            ((LivestockBreed)entity).GestationPeriod = GestationPeriod;
            ((LivestockBreed)entity).HeatPeriod = HeatPeriod;
            ((LivestockBreed)entity).Name = Name;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
