using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class EntitySeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            SeedRoles(builder);
        }
        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = Administration.SystemAdminRole.ToString(), Name = "SystemAdmin", ConcurrencyStamp = "1", NormalizedName = "SystemAdmin".Normalize() },
                new IdentityRole() { Id = Administration.TenantAdminRole.ToString(), Name = "TenantAdmin", ConcurrencyStamp = "1", NormalizedName = "TenantAdmin".Normalize() }
                );
        }
    }
}
