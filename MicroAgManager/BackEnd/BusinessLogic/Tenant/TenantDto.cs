using Domain.Models;

namespace BackEnd.BusinessLogic.Tenant
{
    public class TenantDto 
    {
        public TenantDto(long count, ICollection<TenantModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<TenantModel> Models { get; set; }
    }
}
