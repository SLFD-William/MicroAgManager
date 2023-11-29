using FrontEnd.Persistence;
using MicroAgManager.Client;
using MicroAgManager.Client.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

ClientServices.AddSharedClientServices(builder.Services);

await builder.Build().RunAsync();
