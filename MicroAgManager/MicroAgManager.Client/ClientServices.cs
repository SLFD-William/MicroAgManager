using Domain.Context;
using MicroAgManager.Client.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroAgManager.Client
{
    public static class ClientServices
    {
        public static void AddSharedClientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IAPIService, APIService>();
            serviceCollection.AddDbContextFactory<FrontEndDbContext>(
                options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}")
                );
            serviceCollection.AddSingleton<DataSynchronizer>();
            serviceCollection.AddSingleton<ClientApplicationStateProvider>();
        }
    }
}
