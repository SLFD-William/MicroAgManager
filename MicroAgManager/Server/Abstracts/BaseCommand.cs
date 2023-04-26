using Domain.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Abstracts
{
    public class BaseCommand 
    {
        [Required] public BaseModel Model { get; set; }
        [Required] public Guid ModifiedBy { get; set; }
        [Required] public Guid TenantId { get; set; }
    }
}
