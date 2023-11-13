using Domain.Abstracts;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Abstracts
{
    public abstract class BaseQuery
    {
        public long? Id { get; set; }
        [Required] public Guid TenantId { get; set; }
        public BaseModel? NewModel { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool? GetDeleted { get; set; }

        public DateTime? LastModified { get; set; }
        protected abstract IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context) where T : BaseEntity;
        protected IQueryable<T> PopulateBaseQuery<T>(IQueryable<T> query) where T : BaseEntity
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));
            
            query = query.Where(t => t.TenantId == TenantId).OrderByDescending(_ => _.ModifiedOn);

            if (Id.HasValue)
            {
                query = query.Where(_ => _.Id == Id);
                return query.AsQueryable();
            }

            if (GetDeleted.HasValue && !GetDeleted.Value)
                query = query.Where(_ => !_.DeletedOn.HasValue);

            if (Skip.HasValue || Take.HasValue)
                query = query.Skip(Skip ?? 0).Take(Take ?? 1000);

            if (LastModified.HasValue)
                query = query.Where(_ => _.ModifiedOn > LastModified);

            return query.AsQueryable();
        }

    }
}
