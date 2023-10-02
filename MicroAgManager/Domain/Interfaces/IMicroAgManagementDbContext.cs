using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IMicroAgManagementDbContext:ILoggingDbContext
    {
        DbSet<Tenant> Tenants { get; set; }
        DbSet<FarmLocation> Farms { get; set; }
        DbSet<LandPlot> Plots { get; set; }
        DbSet<Livestock> Livestocks { get; set; }
        DbSet<LivestockAnimal> LivestockAnimals { get; set; }
        DbSet<LivestockBreed> LivestockBreeds { get; set; }
        DbSet<LivestockFeed> LivestockFeeds { get; set; }
        DbSet<LivestockStatus> LivestockStatuses { get; set; }
        DbSet<LivestockFeedServing> LivestockFeedServings { get; set; }
        DbSet<LivestockFeedDistribution> LivestockFeedDistributions { get; set; }
        DbSet<LivestockFeedAnalysis> LivestockFeedAnalyses { get; set; }
        DbSet<LivestockFeedAnalysisParameter> LivestockFeedAnalysisParameters { get; set; }
        DbSet<LivestockFeedAnalysisResult> LivestockFeedAnalysisResults { get; set; }
        DbSet<Duty> Duties { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<Milestone> Milestones { get; set; }
        DbSet<ScheduledDuty> ScheduledDuties { get; set; }
        DbSet<BreedingRecord> BreedingRecords { get; set; }
        DbSet<Registrar> Registrars { get; set; }
        DbSet<Registration> Registrations { get; set; }

    }
}
