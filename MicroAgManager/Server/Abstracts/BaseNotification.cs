using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Abstracts
{
    public class BaseNotification:INotification
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public Guid ModifiedBy { get; set; }
        [Required]
        public Guid TenantId { get; set; }
        [Required]
        public string EntityName { get; set; }

    }
}
