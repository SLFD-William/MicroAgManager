using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IClientDbContext
    {
        public DbSet<FarmLocationModel> Farms { get; set; }
    }
}
