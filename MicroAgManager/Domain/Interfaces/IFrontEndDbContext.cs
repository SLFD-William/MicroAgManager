using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IFrontEndDbContext
    {
        public DbSet<FarmLocationModel> Farms { get; set; }
        public DbSet<TenantModel> Tenants { get; set; }
    }
}
