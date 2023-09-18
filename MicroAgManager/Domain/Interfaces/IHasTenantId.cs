using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces
{
    internal interface IHasTenantId
    {
        [Required] public Guid TenantId { get; set; }
    }
}
