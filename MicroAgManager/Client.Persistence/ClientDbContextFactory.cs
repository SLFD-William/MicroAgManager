using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Persistence
{
    internal class ClientDbContextFactory : DesignTimeDbContextFactoryBase<ClientDbContext>
    {
        protected override ClientDbContext CreateNewInstance(DbContextOptions<ClientDbContext> options)
        {
            return new ClientDbContext(options);
        }
    }
}