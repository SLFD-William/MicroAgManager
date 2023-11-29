using Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Persistence
{
    internal class FrontEndDbContextFactory : DesignTimeDbContextFactoryBase<FrontEndDbContext>
    {
        protected override FrontEndDbContext CreateNewInstance(DbContextOptions<FrontEndDbContext> options)
        {
            return new FrontEndDbContext(options);
        }
    }
}