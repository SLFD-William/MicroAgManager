using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd
{
    //this is to not have the Duende stuff in the back end
    internal class BackEndDbContext : DbContext, IMicroAgManagementDbContext
    {
        public BackEndDbContext(DbContextOptions<BackEndDbContext> options) : base(options)
        {
        }
        public DbSet<Tenant> Tenants {get;set;}
        public DbSet<FarmLocation> Farms {get;set;}
        public DbSet<LandPlot> Plots {get;set;}
        public DbSet<Livestock> Livestocks {get;set;}
        public DbSet<LivestockAnimal> LivestockAnimals {get;set;}
        public DbSet<LivestockBreed> LivestockBreeds {get;set;}
        public DbSet<LivestockFeed> LivestockFeeds {get;set;}
        public DbSet<LivestockStatus> LivestockStatuses {get;set;}
        public DbSet<LivestockFeedServing> LivestockFeedServings {get;set;}
        public DbSet<LivestockFeedDistribution> LivestockFeedDistributions {get;set;}
        public DbSet<LivestockFeedAnalysis> LivestockFeedAnalyses {get;set;}
        public DbSet<LivestockFeedAnalysisParameter> LivestockFeedAnalysisParameters {get;set;}
        public DbSet<LivestockFeedAnalysisResult> LivestockFeedAnalysisResults {get;set;}
        public DbSet<Duty> Duties {get;set;}
        public DbSet<Event> Events {get;set;}
        public DbSet<Milestone> Milestones {get;set;}
        public DbSet<ScheduledDuty> ScheduledDuties {get;set;}
        public DbSet<BreedingRecord> BreedingRecords {get;set;}
        public DbSet<Registrar> Registrars {get;set;}
        public DbSet<Registration> Registrations {get;set;}
        public DbSet<Measure> Measures {get;set;}
        public DbSet<Measurement> Measurements {get;set;}
        public DbSet<Treatment> Treatments {get;set;}
        public DbSet<TreatmentRecord> TreatmentRecords {get;set;}
        public DbSet<Unit> Units {get;set;}
        public DbSet<Log> Logs {get;set;}
    }
}
