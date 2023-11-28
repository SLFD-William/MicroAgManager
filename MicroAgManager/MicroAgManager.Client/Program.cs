using FrontEnd.Persistence;
using MicroAgManager.Client;
using MicroAgManager.Client.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();


builder.Services.AddDbContextFactory<FrontEndDbContext>(
    options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}")
    );
builder.Services.AddScoped<ClientApplicationStateProvider>();

await builder.Build().RunAsync();
