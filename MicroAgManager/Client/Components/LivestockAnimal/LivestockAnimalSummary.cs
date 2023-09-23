using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.LivestockAnimal
{
    public class LivestockAnimalSummary :ValueObject
    {
        private LivestockAnimalModel _livestockAnimalModel;

        public LivestockAnimalSummary(LivestockAnimalModel livestockAnimalModel, FrontEndDbContext context)
        {
            _livestockAnimalModel = livestockAnimalModel ?? throw new ArgumentNullException(nameof(livestockAnimalModel));
            BreedsCount = context.LivestockBreeds.Count(f => f.LivestockAnimalId == livestockAnimalModel.Id);
            StatusesCount = context.LivestockStatuses.Count(f => f.LivestockAnimalId == livestockAnimalModel.Id);
            FeedsCount = context.LivestockFeeds.Count(f => f.LivestockAnimalId == livestockAnimalModel.Id);
        }
        public long Id => _livestockAnimalModel.Id;
        public string Name => _livestockAnimalModel.Name;   
        public string GroupName => _livestockAnimalModel.GroupName; 
        public string ParentFemaleName => _livestockAnimalModel.ParentFemaleName; 
        public string ParentMaleName => _livestockAnimalModel.ParentMaleName; 
        public string Care => _livestockAnimalModel.Care; 
        public int BreedsCount { get; private set; }
        public int StatusesCount { get; private set; }
        public int FeedsCount { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _livestockAnimalModel;
        }
    }
}
