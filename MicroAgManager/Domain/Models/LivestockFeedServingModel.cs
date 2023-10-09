using Domain.Abstracts;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class LivestockFeedServingModel:BaseModel
    {

        [Required][ForeignKey(nameof(LivestockFeedModel))] public long FeedId { get; set; }
        [Required][ForeignKey(nameof(LivestockStatusModel))] public long StatusId { get; set; }
        [Required][MaxLength(50)] public string ServingFrequency { get; set; }
        [Precision(18,3)][Range(1, (double)decimal.MaxValue)] public decimal Serving { get; set; }
        public static LivestockFeedServingModel? Create(LivestockFeedServing livestock)
        {
            var model = PopulateBaseModel(livestock, new LivestockFeedServingModel
            {
                FeedId=livestock.Feed.Id,
                StatusId=livestock.Status.Id,
                ServingFrequency=livestock.ServingFrequency,
                Serving=livestock.Serving
            }) as LivestockFeedServingModel;
            return model;
        }
        

        public override BaseModel Map(BaseModel entity)
        {
            if (entity == null || entity is not LivestockFeedServingModel) return null;
            ((LivestockFeedServingModel)entity).FeedId = FeedId;
            ((LivestockFeedServingModel)entity).StatusId = StatusId;
            ((LivestockFeedServingModel)entity).ServingFrequency = ServingFrequency;
            ((LivestockFeedServingModel)entity).Serving = Serving;
            return entity;
        }

        public override BaseEntity Map(BaseEntity entity)
        {
            if (entity == null || entity is not LivestockFeedServing) return null;
            ((LivestockFeedServing)entity).LivestockFeedId = FeedId;
            ((LivestockFeedServing)entity).LivestockStatusId = StatusId;
            ((LivestockFeedServing)entity).ServingFrequency = ServingFrequency;
            ((LivestockFeedServing)entity).Serving = Serving;
            entity.ModifiedOn = DateTime.UtcNow;
            return entity;
        }
    }
}
