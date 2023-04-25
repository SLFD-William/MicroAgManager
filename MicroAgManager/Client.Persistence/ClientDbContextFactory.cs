using Microsoft.EntityFrameworkCore;

namespace Client.Persistence
{
    internal class ClientDbContextFactory : DesignTimeDbContextFactoryBase<ClientDbContext>
    {
        protected override ClientDbContext CreateNewInstance(DbContextOptions<ClientDbContext> options)
        {
            return new ClientDbContext(options);
        }
    }
}