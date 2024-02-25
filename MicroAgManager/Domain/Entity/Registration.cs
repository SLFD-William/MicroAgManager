using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
    public interface IRegistration
    {
        public long Id { get; set; }
        public DateTime ModifiedOn { get; set; }
        bool DefaultIdentification { get; set; }
        string Identifier { get; set; }
        long RegistrarId { get; set; }
        DateTime RegistrationDate { get; set; }
    }

    [Index(nameof(TenantId))]
    [Index(nameof(ModifiedOn))]
    [Index(nameof(RecipientType), nameof(RecipientTypeId))]
    public class Registration : BaseEntity, IHasRecipient, IRegistration
    {
        public Registration(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        [Required][ForeignKey("Registrar")] public long RegistrarId { get; set; }
        public virtual Registrar Registrar { get; set; }
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required] public long RecipientId { get; set; }
        [Required][MaxLength(40)] public string Identifier { get; set; }
        [Required] public bool DefaultIdentification { get; set; } = false;
        [Required] public DateTime RegistrationDate { get; set; }
    }
}
