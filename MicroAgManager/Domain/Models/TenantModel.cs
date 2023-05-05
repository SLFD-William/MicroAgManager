using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class TenantModel:BaseModel
    {
        [Required] public Guid GuidId { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public Guid? TenantUserAdminId { get; set; }
        public static TenantModel Create(Tenant tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));
            return new TenantModel
            {
                Id = tenant.Id,
                GuidId = tenant.GuidId,
                Name = tenant.Name,
                TenantUserAdminId = tenant.TenantUserAdminId,
                Deleted = false,
                EntityModifiedOn = tenant.ModifiedOn,
                ModifiedOn = tenant.ModifiedOn,
                ModifiedBy = tenant.ModifiedBy
            };
        }
    }
}
