﻿using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.LivestockBreed
{
    public class LivestockBreedSummary:ValueObject
    {
        private LivestockBreedModel _livestockBreedModel;

        public LivestockBreedSummary(LivestockBreedModel livestockBreedModel, FrontEndDbContext context)
        {
            _livestockBreedModel = context.LivestockBreeds.Find(livestockBreedModel.Id);
            LivestockCount = context.Livestocks.Count(c=>c.LivestockBreedId== livestockBreedModel.Id);
        }
        public int LivestockCount { get;private set; }
        public long LivestockAnimalId=>_livestockBreedModel.LivestockAnimalId;
        public long Id => _livestockBreedModel.Id;
        public string Name => _livestockBreedModel.Name;
        public string Emoji => _livestockBreedModel.EmojiChar;
        public int GestationPeriod => _livestockBreedModel.GestationPeriod;
        public int HeatPeriod => _livestockBreedModel.HeatPeriod;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _livestockBreedModel;
        }
    }
}
