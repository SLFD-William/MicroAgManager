using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Abstracts
{
    public abstract class BaseEntity:IHasTenantId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        [Required] public Guid TenantId { get; set; }
        [Required] public DateTime CreatedOn { get; set; }
        [Required] public Guid CreatedBy { get; set; }
        [Required] public DateTime ModifiedOn { get; set; }
        [Required] public Guid ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        protected BaseEntity(Guid createdBy, Guid tenantId)
        {
            var created = DateTime.UtcNow;
            CreatedOn = created;
            ModifiedOn = created;
            CreatedBy = createdBy;
            ModifiedBy = createdBy;
            TenantId = tenantId;
        }
    }
}
