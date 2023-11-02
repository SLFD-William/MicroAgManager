using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.LivestockStatus
{
    public class LivestockStatusSummary:ValueObject
    {
        private LivestockStatusModel _livestockStatusModel;
        

        public LivestockStatusSummary(LivestockStatusModel livestockStatusModel,FrontEndDbContext context)
        {
            _livestockStatusModel = livestockStatusModel;
        }
        public long Id =>_livestockStatusModel.Id;
        public string StatusName => _livestockStatusModel.Status;
        public bool DefaultStatus => _livestockStatusModel.DefaultStatus; 
        public string BeingManaged => _livestockStatusModel.BeingManaged;
        public string Sterile => _livestockStatusModel.Sterile;
        public string InMilk => _livestockStatusModel.InMilk;
        public string BottleFed => _livestockStatusModel.BottleFed;
        public string ForSale => _livestockStatusModel.ForSale;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _livestockStatusModel;
        }
    }
}
