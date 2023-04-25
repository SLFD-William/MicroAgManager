using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Abstracts
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        [Required]
        public Guid TenantId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }

        public DateTime? Deleted { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public Guid ModifiedBy { get; set; }
        public Guid? DeletedBy { get; set; }
        protected BaseEntity(Guid createdBy, Guid tenantId)
        {
            var created = DateTime.Now;
            Created = created;
            ModifiedOn = created;
            CreatedBy = createdBy;
            ModifiedBy = createdBy;
            TenantId = tenantId;
        }
    }
}
