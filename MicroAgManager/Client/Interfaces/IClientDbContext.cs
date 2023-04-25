using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Client.Interfaces
{
    public interface IClientDbContext
    {
        public DbSet<FarmLocationModel> Farms { get; set; }
    }
}
