﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entity
{
    [Index(nameof(GuidId))]
    [Index(nameof(ModifiedOn))]
    public class Tenant
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
