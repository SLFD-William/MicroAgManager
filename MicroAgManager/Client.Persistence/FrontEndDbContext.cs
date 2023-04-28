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
    }
}
