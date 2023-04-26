using Domain.Interfaces;
using Domain.Models;
using BackEnd.Abstracts;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class FarmLocationsQueries : BaseQuery
    {

        public FarmLocationModel? NewAnimalFarmLocation { get => (FarmLocationModel?)NewModel; set => NewModel = value; }
        public bool? GetDeleted { get; set; }

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
            var query = context.Farms.Where(f => f.TenantId == TenantId).AsQueryable();
            if(query is null)
                throw new ArgumentNullException(nameof(query));
            if (!(GetDeleted ?? false))
                query = query.Where(_ => !_.Deleted.HasValue);
            if (Take.HasValue && Take > 0)
                query = query.Skip(Skip ?? 0).Take(Take.Value);
            if (Name != null)
                query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (Longitude != null)
                query = query.Where(_ => _.Longitude != null && _.Longitude.Contains(Longitude));
            if (Latitude != null)
                query = query.Where(_ => _.Latitude != null && _.Latitude.Contains(Latitude));
            if (StreetAddress != null)
                query = query.Where(_ => _.StreetAddress != null && _.StreetAddress.Contains(StreetAddress));
            if (City != null)
                query = query.Where(_ => _.City != null && _.City.Contains(City));
            if (State != null)
                query = query.Where(_ => _.State != null && _.State.Contains(State));
            if (Zip != null)
                query = query.Where(_ => _.Zip != null && _.Zip.Contains(Zip));
            if (Country != null)
                query = query.Where(_ => _.Country != null && _.Country.Contains(Country));
            if (LastModified.HasValue)
                query = query.Where(_ => _.ModifiedOn >= LastModified);
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }
    }
}
