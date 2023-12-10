using FrontEnd.Persistence;
using MicroAgManager.Client.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroAgManager.Client
{
    public static class ClientServices
    {
        public static void AddSharedClientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddGeolocationServices();
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IAPIService, APIService>();
            serviceCollection.AddDbContextFactory<FrontEndDbContext>(
                options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}")
                );
            serviceCollection.AddSingleton<DataSynchronizer>();
            serviceCollection.AddQuickGridEntityFrameworkAdapter();
            serviceCollection.AddSingleton<ClientApplicationStateProvider>();
        }
    }
}
