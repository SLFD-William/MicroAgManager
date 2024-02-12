using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
    public interface ITenant
    {
        string AccessLevel { get; set; }
        DateTime Created { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? Deleted { get; set; }
        Guid? DeletedBy { get; set; }
        Guid GuidId { get; set; }
        long Id { get; set; }
        Guid ModifiedBy { get; set; }
        DateTime ModifiedOn { get; set; }
        string Name { get; set; }
        Guid TenantUserAdminId { get; set; }
        string? WeatherServiceQueryURL { get; set; }
    }

    [Index(nameof(GuidId))]
    [Index(nameof(ModifiedOn))]
    public class Tenant : ITenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        [Required]
        public Guid GuidId { get; set; }
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
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public Guid TenantUserAdminId { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        public string? WeatherServiceQueryURL { get; set; }
        public Tenant(Guid createdBy)
        {
            Created = DateTime.Now;
            CreatedBy = createdBy;
            ModifiedOn = Created;
            ModifiedBy = createdBy;
            AccessLevel = nameof(TennantAccessLevelConstants.SingleUser);
        }
    }
}
