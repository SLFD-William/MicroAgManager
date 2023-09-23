using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.Farm
{
    public class FarmLocationSummary:ValueObject
    {
        private FarmLocationModel _farmModel;
        
        public FarmLocationSummary(FarmLocationModel farmModel, FrontEndDbContext context)
        {
            _farmModel = farmModel;
        }
        public int PlotCount { get; private set; }
        public int LivestockAnimalCount { get; private set; }
        public int LivestockCount { get; private set; }
        public int DutyCount { get; private set; }
        public long Id => _farmModel.Id;
        public string Name => _farmModel.Name;
        public string StreetAddress => _farmModel.StreetAddress;
        public string City => _farmModel.City;
        public string State => _farmModel.State;
        public string Zip => _farmModel.Zip;
        public string Country => _farmModel.Country;
        public string CountryCode => _farmModel.CountryCode;
        public double? Latitude => _farmModel.Latitude;
        public double? Longitude => _farmModel.Longitude;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _farmModel;
        }
    }
}
