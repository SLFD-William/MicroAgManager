using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Abstracts
{
    public class BaseCommand : IRequest<long>
    {
        [Required] public Guid ModifiedBy { get; set; }
        [Required] public Guid TenantId { get; set; }
    }
}
