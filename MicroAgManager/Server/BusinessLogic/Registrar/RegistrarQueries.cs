using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Registrar
{
    public class RegistrarQueries : BaseQuery
    {
        public RegistrarModel NewRegistrar { get => (RegistrarModel)NewModel; set => NewModel = value; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? API { get; set; }
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Registrars.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (!string.IsNullOrEmpty(Email)) query = query.Where(_ => _.Email != null && _.Email.Contains(Email));
            if (!string.IsNullOrEmpty(Website)) query = query.Where(_ => _.Website != null && _.Website.Contains(Website));
            if (!string.IsNullOrEmpty(API)) query = query.Where(_ => _.API != null && _.API.Contains(API));
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return (IQueryable<T>)query;
        }
    }
}
