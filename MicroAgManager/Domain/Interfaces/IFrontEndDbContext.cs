using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IFrontEndDbContext
    {
        DbSet<FarmLocationModel> Farms { get; set; }
        DbSet<LandPlotModel> LandPlots { get; set; }
        DbSet<TenantModel> Tenants { get; set; }
        DbSet<LivestockModel> Livestocks { get; set; }
        DbSet<LivestockTypeModel> LivestockTypes { get; set; }
        DbSet<LivestockBreedModel> LivestockBreeds { get; set; }
        DbSet<LivestockFeedModel> LivestockFeeds { get; set; }
        DbSet<LivestockStatusModel> LivestockStatuses { get; set; }
        DbSet<LivestockFeedServingModel> LivestockFeedServings { get; set; }
        DbSet<LivestockFeedDistributionModel> LivestockFeedDistributions { get; set; }
        DbSet<LivestockFeedAnalysisModel> LivestockFeedAnalyses { get; set; }
        DbSet<LivestockFeedAnalysisParameterModel> LivestockFeedAnalysisParameters { get; set; }
        DbSet<LivestockFeedAnalysisResultModel> LivestockFeedAnalysisResults { get; set; }
    }
}
