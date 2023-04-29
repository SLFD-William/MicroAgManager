using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class LivestockFeedModel : BaseModel
    {
        [Required][ForeignKey(nameof(LivestockTypeModel))] public long LivestockTypeId { get; set; }
        [Required][MaxLength(255)]public string Name { get; set; }
        [Required][MaxLength(255)] public string Source { get; set; }
        [Range(1, int.MaxValue)] public int? Cutting { get; set; } //Hay Only
        //[RequiredIf("AnimalFeedType", "Hay")]
        [Required]public bool? Active { get; set; }
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
                LivestockTypeId=livestockFeed.LivestockType.Id,
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
        public LivestockFeed MapToEntity(LivestockFeed entity)
        {
            entity.Active = Active;
            entity.Source = Source;
            entity.Quantity = Quantity;
            entity.QuantityUnit = QuantityUnit;
            entity.QuantityWarning = QuantityWarning;
            entity.FeedType = FeedType;
            entity.Distribution = Distribution;
            entity.Name = Name;
            entity.LivestockType.Id = LivestockTypeId;
            if (entity.Servings?.Any() ?? false)
                foreach (var breed in entity.Servings)
                    Servings?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            if (entity.Distributions?.Any() ?? false)
                foreach (var breed in entity.Distributions)
                    Distributions?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            return entity;
        }
    }
}
