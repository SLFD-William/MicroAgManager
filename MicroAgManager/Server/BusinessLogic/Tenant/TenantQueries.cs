using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Tenant
{
    public class TenantQueries : BaseQuery
    {
        public string? Name { get; set; }
        public Guid? TenantUserAdminId { get; set; }
        public TenantModel? NewTenant { get => (TenantModel?)NewModel; set => NewModel = value; }

        protected IQueryable<Domain.Entity.Tenant> GetQuery(IMicroAgManagementDbContext context)
        { 
            var query=context.Tenants.Where(t=>t.GuidId == TenantId).AsQueryable();
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(_ => _.Name != null && _.Name.Contains(Name));

            if (TenantUserAdminId.HasValue)
                query = query.Where(_ => _.TenantUserAdminId== TenantUserAdminId);

            if (LastModified.HasValue)
                query = query.Where(_ => _.ModifiedOn >= LastModified);
            
            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}
