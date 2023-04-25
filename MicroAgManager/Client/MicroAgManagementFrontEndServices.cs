using FrontEnd.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FrontEnd
{
    public static class MicroAgManagementFrontEndServices
    {
        public static void AddMicroAgManagementFrontEndServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextFactory<FrontEndDbContext>(
                options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}")
                );
            serviceCollection.AddScoped<DataSynchronizer>();
            serviceCollection.AddScoped<FrontEndAuthenticationStateProvider>();
            serviceCollection.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<FrontEndAuthenticationStateProvider>());
        }
    }
}
