using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class TenantModel:BaseModel
    {
        [Required] public new Guid Id { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public Guid? TenantUserAdminId { get; set; }
        public virtual ICollection<FarmLocationModel?>? Farms { get; set; }
        public static TenantModel Create(Tenant tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));
            return new TenantModel
            {
                Id = tenant.Id,
                Name = tenant.Name,
                TenantUserAdminId = tenant.TenantUserAdminId,
                Farms = tenant.Farms?.Select(FarmLocationModel.Create).ToList() ?? new List<FarmLocationModel?>(),
                Deleted = false,
                EntityModifiedOn = tenant.ModifiedOn,
                ModifiedOn = tenant.ModifiedOn,
                ModifiedBy = tenant.ModifiedBy
            };
        }
    }
}
