using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
