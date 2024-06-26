﻿
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class ApplicationUser : IdentityUser, IHasTenantId
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<IdentityUserClaim<string>>? Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>>? Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>>? Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>>? UserRoles { get; set; }
        [Required]public Guid TenantId { get; set ; }

    }
}
