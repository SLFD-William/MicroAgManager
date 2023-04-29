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
        public LivestockFeedServing MapToEntity(LivestockFeedServing entity)
        {
            entity.Feed.Id = FeedId;
            entity.Status.Id = StatusId;
            entity.ServingFrequency = ServingFrequency;
            entity.Serving = Serving;
            return entity;
        }
    }
}
