using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class MicroAgManagementDbContextFactory : DesignTimeDbContextFactoryBase<MicroAgManagementDbContext>
    {
        protected override MicroAgManagementDbContext CreateNewInstance(DbContextOptions<MicroAgManagementDbContext> options)
        {
            return new MicroAgManagementDbContext(options);
        }
    }
}
