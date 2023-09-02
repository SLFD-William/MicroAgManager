using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IFrontEndDbContext:ILoggingDbContext
    {
        DbSet<FarmLocationModel> Farms { get; set; }
        DbSet<LandPlotModel> LandPlots { get; set; }
        DbSet<TenantModel> Tenants { get; set; }
        DbSet<LivestockModel> Livestocks { get; set; }
        DbSet<LivestockAnimalModel> LivestockAnimals { get; set; }
        DbSet<LivestockBreedModel> LivestockBreeds { get; set; }
        DbSet<LivestockFeedModel> LivestockFeeds { get; set; }
        DbSet<LivestockStatusModel> LivestockStatuses { get; set; }
        DbSet<LivestockFeedServingModel> LivestockFeedServings { get; set; }
        DbSet<LivestockFeedDistributionModel> LivestockFeedDistributions { get; set; }
        DbSet<LivestockFeedAnalysisModel> LivestockFeedAnalyses { get; set; }
        DbSet<LivestockFeedAnalysisParameterModel> LivestockFeedAnalysisParameters { get; set; }
        DbSet<LivestockFeedAnalysisResultModel> LivestockFeedAnalysisResults { get; set; }
        public DbSet<DutyModel> Duties { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<MilestoneModel> Milestones { get; set; }
        public DbSet<ScheduledDutyModel> ScheduledDuties { get; set; }
    }
}
