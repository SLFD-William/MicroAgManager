using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Registrar : BaseEntity
    {
        public Registrar(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][MaxLength(40)] public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string API { get; set; } = string.Empty;
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
