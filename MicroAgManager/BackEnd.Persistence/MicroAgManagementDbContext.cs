using Domain.Entity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class MicroAgManagementDbContext : IdentityDbContext<ApplicationUser>, IMicroAgManagementDbContext
    {
        private DbContextOptions<MicroAgManagementDbContext> options;

        public MicroAgManagementDbContext(DbContextOptions<MicroAgManagementDbContext> options) : base(options)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<FarmLocation> Farms { get; set; }
        public DbSet<LandPlot> Plots { get; set; }
        public DbSet<Livestock> Livestocks { get; set; }
        public DbSet<LivestockAnimal> LivestockAnimals { get; set; }
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
        public DbSet<Log> Logs { get; set; }
        public DbSet<BreedingRecord> BreedingRecords { get; set; }
        public DbSet<Registrar> Registrars { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentRecord> TreatmentRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LivestockFeed>()
                .HasMany(lf => lf.Servings)
                .WithOne(s => s.Feed)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<LivestockStatus>()
                .HasMany(lf => lf.FeedServings)
                .WithOne(s => s.Status)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<LivestockAnimal>()
                .HasMany(lf => lf.Statuses)
                .WithOne(s => s.LivestockAnimal)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Measure>()
                .HasOne(u=>u.Unit).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Treatment>()
                .HasOne(u => u.RecipientMassUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Treatment>()
                .HasOne(u => u.DosageUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Treatment>()
                .HasOne(u => u.DurationUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Treatment>()
                .HasOne(u => u.FrequencyUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TreatmentRecord>()
                .HasOne(u => u.DosageUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<LandPlot>()
                .HasOne(u => u.AreaUnit).WithMany().OnDelete(DeleteBehavior.NoAction);


            //EntitySeeder.Seed(modelBuilder);
        }
    }
}
