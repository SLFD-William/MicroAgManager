using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IMicroAgManagementDbContext
    {
        DbSet<Tenant> Tenants { get; set; }
        DbSet<FarmLocation> Farms { get; set; }
        DbSet<LandPlot> Plots { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
