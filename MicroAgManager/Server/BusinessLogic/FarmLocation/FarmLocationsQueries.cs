using Domain.Interfaces;
using Domain.Models;
using BackEnd.Abstracts;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class FarmLocationsQueries : BaseQuery
    {

        public FarmLocationModel? NewAnimalFarmLocation { get => (FarmLocationModel?)NewModel; set => NewModel = value; }
        public string? Name { get; set; }
        public long? Longitude { get; set; }
        public long? Latitude { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Farms.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (Longitude.HasValue) query = query.Where(_ => _.Longitude != null && _.Longitude==Longitude.Value);
            if (Latitude.HasValue) query = query.Where(_ => _.Latitude != null && _.Latitude==Latitude.Value);
            if (!string.IsNullOrEmpty(StreetAddress)) query = query.Where(_ => _.StreetAddress != null && _.StreetAddress.Contains(StreetAddress));
            if (!string.IsNullOrEmpty(City)) query = query.Where(_ => _.City != null && _.City.Contains(City));
            if (!string.IsNullOrEmpty(State)) query = query.Where(_ => _.State != null && _.State.Contains(State));
            if (!string.IsNullOrEmpty(Zip)) query = query.Where(_ => _.Zip != null && _.Zip.Contains(Zip));
            if (!string.IsNullOrEmpty(Country)) query = query.Where(_ => _.Country != null && _.Country.Contains(Country));
            if (!string.IsNullOrEmpty(CountryCode)) query = query.Where(_ => _.CountryCode != null && _.CountryCode.Contains(CountryCode));
            
            
            return (IQueryable<T>)query;
        }
    }
}
