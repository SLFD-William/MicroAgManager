﻿using Domain.Entity;
using Domain.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

            EntitySeeder.Seed(modelBuilder);
        }
    }
}
