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
        public DbSet<Livestock> Livestocks { get; set; }
        public DbSet<LivestockType> LivestockTypes { get; set; }
        public DbSet<LivestockBreed> LivestockBreeds { get; set; }
        public DbSet<LivestockFeed> LivestockFeeds { get; set; }
        public DbSet<LivestockStatus> LivestockStatuses { get; set; }
        public DbSet<LivestockFeedServing> LivestockFeedServings { get; set; }
        public DbSet<LivestockFeedDistribution> LivestockFeedDistributions { get; set; }
        public DbSet<LivestockFeedAnalysis> LivestockFeedAnalyses { get; set; }
        public DbSet<LivestockFeedAnalysisParameter> LivestockFeedAnalysisParameters { get; set; }
        public DbSet<LivestockFeedAnalysisResult> LivestockFeedAnalysisResults { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<ScheduledDuty> ScheduledDuties { get; set; }

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
