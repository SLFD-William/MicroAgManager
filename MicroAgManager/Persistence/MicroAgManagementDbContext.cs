using Domain.Entity;
using Domain.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Configurations;

namespace Persistence
{
    public class MicroAgManagementDbContext : ApiAuthorizationDbContext<ApplicationUser>, IMicroAgManagementDbContext
    {
        private DbContextOptions<MicroAgManagementDbContext> options;

        public MicroAgManagementDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<FarmLocation> Farms { get; set; }
        public DbSet<LandPlot> Plots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .Navigation(e => e.Tenant)
                .AutoInclude();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantConfiguration).Assembly);
            EntitySeeder.Seed(modelBuilder);
        }
    }
}
