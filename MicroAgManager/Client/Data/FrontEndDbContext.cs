using FrontEnd.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Data
{
    public class FrontEndDbContext : DbContext, IFrontEndDbContext
    {
        
        public FrontEndDbContext(DbContextOptions options) : base(options) { }

        public DbSet<FarmLocationModel> Farms { get; set; }
    }
}
