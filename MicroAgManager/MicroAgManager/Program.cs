using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MicroAgManager.Components;
using MicroAgManager.Identity;
using MicroAgManager.Data;
using FrontEnd.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



// register the cookie handler
builder.Services.AddScoped<CookieHandler>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// set base address for default host
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002") });

// configure client for API interactions
builder.Services.AddHttpClient(
    "API",
    opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddGeolocationServices();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddDbContextFactory<FrontEndDbContext>(options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}"));

builder.Services.AddScoped<DataSynchronizer>();
builder.Services.AddQuickGridEntityFrameworkAdapter();
await builder.Build().RunAsync();
