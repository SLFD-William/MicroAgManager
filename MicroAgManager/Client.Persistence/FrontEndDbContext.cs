using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Persistence
{
    public class FrontEndDbContext : DbContext, IFrontEndDbContext
    {
        
        public FrontEndDbContext(DbContextOptions options) : base(options) { }

        public DbSet<FarmLocationModel> Farms { get; set; }
        public DbSet<TenantModel> Tenants { get; set; }
        public DbSet<LandPlotModel> LandPlots { get; set; }
        public DbSet<LivestockModel> Livestocks { get; set; }
        public DbSet<LivestockTypeModel> LivestockTypes { get; set; }
        public DbSet<LivestockBreedModel> LivestockBreeds { get; set; }
        public DbSet<LivestockFeedModel> LivestockFeeds { get; set; }
        public DbSet<LivestockStatusModel> LivestockStatuses { get; set; }
        public DbSet<LivestockFeedServingModel> LivestockFeedServings { get; set; }
        public DbSet<LivestockFeedDistributionModel> LivestockFeedDistributions { get; set; }
        public DbSet<LivestockFeedAnalysisModel> LivestockFeedAnalyses { get; set; }
        public DbSet<LivestockFeedAnalysisParameterModel> LivestockFeedAnalysisParameters { get; set; }
        public DbSet<LivestockFeedAnalysisResultModel> LivestockFeedAnalysisResults { get; set; }
    }
}
