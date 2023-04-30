﻿using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class LivestockFeedAnalysisModel : BaseModel
    {
        [MaxLength(40)]public string LabNumber { get; set; }

        [Required]
        [ForeignKey(nameof(LivestockFeedModel))]
        public long FeedId { get; set; }
        [MaxLength(40)] public string TestCode { get; set; }
        public DateTime? DateSampled { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateReported { get; set; }
        public DateTime? DatePrinted { get; set; }
        public ICollection<LivestockFeedAnalysisResultModel?> Results { get; set; } = new List<LivestockFeedAnalysisResultModel?>();
        public static LivestockFeedAnalysisModel? Create(LivestockFeedAnalysis livestockBreed)
        {
            var model = PopulateBaseModel(livestockBreed, new LivestockFeedAnalysisModel
            {
                LabNumber = livestockBreed.LabNumber,
                FeedId = livestockBreed.Feed.Id,
                DatePrinted = livestockBreed.DatePrinted,
                DateReceived = livestockBreed.DateReceived,
                DateReported = livestockBreed.DateReported,
                DateSampled = livestockBreed.DateSampled,
                TestCode = livestockBreed.TestCode,
                Results = livestockBreed.Results.Select(LivestockFeedAnalysisResultModel.Create).ToList() ?? new List<LivestockFeedAnalysisResultModel?>()
            }) as LivestockFeedAnalysisModel;
            return model;
        }
        public LivestockFeedAnalysis MapToEntity(LivestockFeedAnalysis entity)
        {
            entity.LabNumber = LabNumber;
            entity.DateReceived = DateReceived;
            entity.DateReported = DateReported;
            entity.DateSampled = DateSampled;
            entity.TestCode = TestCode;
            entity.DatePrinted = DatePrinted;
            entity.Feed.Id = FeedId;
            if (entity.Results?.Any() ?? false)
                foreach (var breed in entity.Results)
                    Results?.FirstOrDefault(p => p?.Id == breed.Id)?.MapToEntity(breed);
            return entity;
        }
    }
}