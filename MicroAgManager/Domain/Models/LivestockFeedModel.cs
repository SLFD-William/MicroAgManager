using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class LivestockFeedModel : BaseModel
    {
        [Required][ForeignKey(nameof(LivestockAnimalModel))] public long LivestockAnimalId { get; set; }
        [Required][MaxLength(255)]public string Name { get; set; }
        [Required][MaxLength(255)] public string Source { get; set; }
        [Range(1, int.MaxValue)] public int? Cutting { get; set; } //Hay Only
        //[RequiredIf("AnimalFeedType", "Hay")]
        [Required]public bool Active { get; set; }
        [Required][Precision(18, 3)][Range(0, (double)decimal.MaxValue)] public decimal Quantity { get; set; }
        [Required][MaxLength(20)] public string QuantityUnit { get; set; }
        [Required][Precision(18, 3)][Range(0, (double)decimal.MaxValue)] public decimal QuantityWarning { get; set; }
        [Required][MaxLength(20)] public string FeedType { get; set; }
        [Required][MaxLength(20)] public string Distribution { get; set; }
        public virtual ICollection<LivestockFeedServingModel?> Servings { get; set; } = new List<LivestockFeedServingModel?>();
        public virtual ICollection<LivestockFeedDistributionModel?> Distributions { get; set; } = new List<LivestockFeedDistributionModel?>();
        public static LivestockFeedModel? Create(LivestockFeed livestockFeed)
        {
            var model = PopulateBaseModel(livestockFeed, new LivestockFeedModel
            {
                LivestockAnimalId=livestockFeed.LivestockAnimal.Id,
                Name=livestockFeed.Name,
                Source=livestockFeed.Source,
                Quantity=livestockFeed.Quantity,
                QuantityUnit=livestockFeed.QuantityUnit,
                QuantityWarning=livestockFeed.QuantityWarning,
                Active=livestockFeed.Active,
                FeedType=livestockFeed.FeedType,
                Distribution=livestockFeed.Distribution,
                Servings = livestockFeed.Servings.Select(LivestockFeedServingModel.Create).ToList() ?? new List<LivestockFeedServingModel?>(),
                Distributions = livestockFeed.Distributions.Select(LivestockFeedDistributionModel.Create).ToList() ?? new List<LivestockFeedDistributionModel?>(),
            }) as LivestockFeedModel;
            return model;
        }
        

        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockFeedModel) return null;
            ((LivestockFeedModel)entity).Active = Active;
            ((LivestockFeedModel)entity).Source = Source;
            ((LivestockFeedModel)entity).Quantity = Quantity;
            ((LivestockFeedModel)entity).QuantityUnit = QuantityUnit;
            ((LivestockFeedModel)entity).QuantityWarning = QuantityWarning;
            ((LivestockFeedModel)entity).FeedType = FeedType;
            ((LivestockFeedModel)entity).Distribution = Distribution;
            ((LivestockFeedModel)entity).Name = Name;
            ((LivestockFeedModel)entity).LivestockAnimalId = LivestockAnimalId;
            if (((LivestockFeedModel)entity).Servings?.Any() ?? false)
                foreach (var breed in ((LivestockFeedModel)entity).Servings)
                    Servings?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            if (((LivestockFeedModel)entity).Distributions?.Any() ?? false)
                foreach (var breed in ((LivestockFeedModel)entity).Distributions)
                    Distributions?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockFeed) return null;
            ((LivestockFeed)entity).Active = Active;
            ((LivestockFeed)entity).Source = Source;
            ((LivestockFeed)entity).Quantity = Quantity;
            ((LivestockFeed)entity).QuantityUnit = QuantityUnit;
            ((LivestockFeed)entity).QuantityWarning = QuantityWarning;
            ((LivestockFeed)entity).FeedType = FeedType;
            ((LivestockFeed)entity).Distribution = Distribution;
            ((LivestockFeed)entity).Name = Name;
            ((LivestockFeed)entity).LivestockAnimalId = LivestockAnimalId;
            entity.ModifiedOn = DateTime.UtcNow;
            if (((LivestockFeed)entity).Servings?.Any() ?? false)
                foreach (var breed in ((LivestockFeed)entity).Servings)
                    Servings?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            if (((LivestockFeed)entity).Distributions?.Any() ?? false)
                foreach (var breed in ((LivestockFeed)entity).Distributions)
                    Distributions?.FirstOrDefault(p => p?.Id == breed.Id)?.Map(breed);
            return entity;
        }
    }
}
