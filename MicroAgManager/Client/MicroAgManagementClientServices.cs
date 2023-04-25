using Client.Data;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public static class MicroAgManagementClientServices
    {
        public static void AddMicroAgManagementClientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextFactory<ClientDbContext>(
                options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}")
                );
            serviceCollection.AddScoped<DataSynchronizer>();
            serviceCollection.AddScoped<ClientAuthenticationStateProvider>();
            serviceCollection.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ClientAuthenticationStateProvider>());
        }
    }
}
