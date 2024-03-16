using Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public interface IRegistrar
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        string API { get; set; }
        string Email { get; set; }
        string Name { get; set; }
        string RegistrarFarmID { get; set; }
       ICollection<IRegistration>? Registrations { get; set; }
        string Website { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    public class Registrar : BaseEntity, IRegistrar
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
         ICollection<IRegistration>? IRegistrar.Registrations { get => Registrations as ICollection<IRegistration>; set => Registrations = value as ICollection<Registration> ?? new List<Registration>(); }
    }
}
