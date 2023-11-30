using BackEnd.Infrastructure;
using Domain.Context;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logging;
using MediatR;
using MicroAgManager;
using MicroAgManager.Client;
using MicroAgManager.Client.Pages;
using MicroAgManager.Components;
using MicroAgManager.Components.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<IMicroAgManagementDbContext,MicroAgManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddLogging(bu =>
{
    bu.AddConfiguration(builder.Configuration.GetSection("Logging"));
    bu.AddProvider(new DatabaseLoggingProvider(builder.Services.BuildServiceProvider().GetService<MicroAgManagementDbContext>(), builder.Configuration));
});
builder.Services.AddSingleton(typeof(ILogger), typeof(Logger<Log>));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MicroAgManagementDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<IClaimsTransformation, ApiClaimsTransformation>();
ClientServices.AddSharedClientServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Farm).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();
BusinessLogicAPI.MapTest(app);
BusinessLogicAPI.MapAnciliary(app);
BusinessLogicAPI.MapFarm(app);
BusinessLogicAPI.MapJoins(app);
BusinessLogicAPI.MapLivestock(app);
BusinessLogicAPI.MapScheduling(app);


app.Run();
