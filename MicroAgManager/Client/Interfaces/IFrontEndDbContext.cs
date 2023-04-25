using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Interfaces
{
    public interface IFrontEndDbContext
    {
        public DbSet<FarmLocationModel> Farms { get; set; }
    }
}
