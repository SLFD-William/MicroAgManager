using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.Livestock
{
    public class LivestockSummary:ValueObject
    {
        private LivestockModel _livestockModel;
        public LivestockSummary(LivestockModel livestockModel, FrontEndDbContext context)
        {
            _livestockModel = livestockModel;
            Status = context.LivestockStatuses.Find(livestockModel.StatusId).Status;
        }
        public int LivestockCount { get; private set; }
        public string Status { get; private set; }
        public long Id => _livestockModel.Id;
        public string Name => _livestockModel.Name;
        public string Gender => _livestockModel.Gender;
        
        public string BatchNumber => _livestockModel.BatchNumber;
        public string Variety => _livestockModel.Variety;
        public DateTime Birthdate => _livestockModel.Birthdate;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _livestockModel;
        }
    }
}
