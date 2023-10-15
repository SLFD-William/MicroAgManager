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
            BreedsCount = context.LivestockBreeds.Count(b=>b.LivestockAnimalId== livestockAnimalModel.Id);
            StatusesCount = context.LivestockStatuses.Count(b => b.LivestockAnimalId == livestockAnimalModel.Id);
            FeedsCount = context.LivestockFeeds.Count(b => b.LivestockAnimalId == livestockAnimalModel.Id);
            EntityName = livestockAnimalModel.GetType().Name.Replace("Model",string.Empty);
        }
        public long Id => _livestockAnimalModel.Id;
        public string Name => _livestockAnimalModel.Name;   
        public string GroupName => _livestockAnimalModel.GroupName; 
        public string ParentFemaleName => _livestockAnimalModel.ParentFemaleName; 
        public string ParentMaleName => _livestockAnimalModel.ParentMaleName; 
        public string Care => _livestockAnimalModel.Care; 
        public string EntityName { get; private set; }
        public int BreedsCount { get; private set; }
        public int StatusesCount { get; private set; }
        public int FeedsCount { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _livestockAnimalModel;
        }
    }
}
