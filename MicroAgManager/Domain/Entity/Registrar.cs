using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Registrar : BaseEntity
    {
        public Registrar(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(40)] public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string API { get; set; } = string.Empty;
        [Required][MaxLength(40)] public string RegistrarFarmID { get; set; } = string.Empty;
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
