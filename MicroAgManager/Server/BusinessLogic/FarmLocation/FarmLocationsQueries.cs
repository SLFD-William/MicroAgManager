using Domain.Interfaces;
using Domain.Models;
using BackEnd.Abstracts;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class FarmLocationsQueries : BaseQuery
    {

        public FarmLocationModel? NewAnimalFarmLocation { get => (FarmLocationModel?)NewModel; set => NewModel = value; }


        public string? Name { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        

        public IQueryable<Domain.Entity.FarmLocation> GetQuery(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Farms.AsQueryable());
            if(query is null) throw new ArgumentNullException(nameof(query));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (!string.IsNullOrEmpty(Longitude)) query = query.Where(_ => _.Longitude != null && _.Longitude.Contains(Longitude));
            if (!string.IsNullOrEmpty(Latitude)) query = query.Where(_ => _.Latitude != null && _.Latitude.Contains(Latitude));
            if (!string.IsNullOrEmpty(StreetAddress)) query = query.Where(_ => _.StreetAddress != null && _.StreetAddress.Contains(StreetAddress));
            if (!string.IsNullOrEmpty(City)) query = query.Where(_ => _.City != null && _.City.Contains(City));
            if (!string.IsNullOrEmpty(State)) query = query.Where(_ => _.State != null && _.State.Contains(State));
            if (!string.IsNullOrEmpty(Zip)) query = query.Where(_ => _.Zip != null && _.Zip.Contains(Zip));
            if (!string.IsNullOrEmpty(Country)) query = query.Where(_ => _.Country != null && _.Country.Contains(Country));
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }
    }
}
