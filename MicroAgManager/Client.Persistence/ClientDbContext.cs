using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Client.Persistence
{
    public class ClientDbContext : DbContext, IClientDbContext
    {
        
        public ClientDbContext(DbContextOptions options) : base(options) { }

        public DbSet<FarmLocationModel> Farms { get; set; }
    }
}
