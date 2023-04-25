using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Persistence
{
    public class MicroAgManagementDbContextFactory : DesignTimeDbContextFactoryBase<MicroAgManagementDbContext>
    {
        protected override MicroAgManagementDbContext CreateNewInstance(DbContextOptions<MicroAgManagementDbContext> options)
        {
            return new MicroAgManagementDbContext(options, new OperationalStoreOptionsMigrations());
        }
    }
    public class OperationalStoreOptionsMigrations :
   IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value => new OperationalStoreOptions()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
            EnableTokenCleanup = false,
            PersistedGrants = new TableConfiguration("PersistedGrants"),
            TokenCleanupBatchSize = 100,
            TokenCleanupInterval = 3600,
        };
    }
}
