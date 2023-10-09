using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Domain.Extensions;

namespace Domain.Models
{
    public class LivestockFeedDistributionModel:BaseModel
    {
        [Required][ForeignKey(nameof(LivestockFeedModel))] public long FeedId { get; set; }
        [Precision(18,3)][Range(0, (double)decimal.MaxValue)] public decimal Quantity { get; set; }
        [Required]public bool? Discarded { get; set; }
        [RequiredIf("Discarded", "True")]
        [MinLength(5)][MaxLength(50)]public string Note { get; set; }
        [Required] public DateTime DatePerformed { get; set; }
        public static LivestockFeedDistributionModel? Create(LivestockFeedDistribution livestock)
        {
            var model = PopulateBaseModel(livestock, new LivestockFeedDistributionModel
            {
                FeedId=livestock.Feed.Id,
                Quantity=livestock.Quantity,
                Discarded=livestock.Discarded,
                Note=livestock.Note,
                DatePerformed=livestock.DatePerformed,
            }) as LivestockFeedDistributionModel;
            return model;
        }

        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockFeedDistributionModel) return null;
           ((LivestockFeedDistributionModel) entity).Quantity = Quantity;
            ((LivestockFeedDistributionModel)entity).Discarded = Discarded ?? false;
            ((LivestockFeedDistributionModel)entity).Note = Note;
            ((LivestockFeedDistributionModel)entity).DatePerformed = DatePerformed;
            ((LivestockFeedDistributionModel)entity).FeedId = FeedId;
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockFeedDistribution) return null;
            ((LivestockFeedDistribution)entity).Quantity = Quantity;
            ((LivestockFeedDistribution)entity).Discarded = Discarded ?? false;
            ((LivestockFeedDistribution)entity).Note = Note;
            ((LivestockFeedDistribution)entity).DatePerformed = DatePerformed;
            ((LivestockFeedDistribution)entity).Feed.Id = FeedId;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
