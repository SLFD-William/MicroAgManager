using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logging;
using MediatR;
using MicroAgManager.API;
using MicroAgManager.API.Hubs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));

// cookie authentication
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

// configure authorization
builder.Services.AddAuthorizationBuilder();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<IMicroAgManagementDbContext, MicroAgManagementDbContext>(options =>options.UseSqlServer(connectionString));

builder.Services.AddLogging(bu =>
{
    bu.AddConfiguration(builder.Configuration.GetSection("Logging"));
    bu.AddProvider(new DatabaseLoggingProvider(builder.Services.BuildServiceProvider().GetService<MicroAgManagementDbContext>(), builder.Configuration));
});
builder.Services.AddSingleton(typeof(ILogger), typeof(Logger<Log>));


// add identity and opt-in to endpoints
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MicroAgManagementDbContext>()
    .AddApiEndpoints();

builder.Services.AddTransient<IClaimsTransformation, ApiClaimsTransformation>();

builder.Services.AddTransient<IEmailSender, EmailServices>(i =>
                new EmailServices(
                    builder.Configuration["EmailServices:Host"],
                    builder.Configuration.GetValue<int>("EmailServices:Port"),
                    builder.Configuration.GetValue<bool>("EmailServices:EnableSSL"),
                    builder.Configuration["EmailServices:UserName"],
                    builder.Configuration["EmailServices:Password"]
                ));

// add CORS policy for Wasm client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:5001",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:5002"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));

// Add services to the container.

builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; })
               .AddMessagePackProtocol();

builder.Services.AddScoped<INotificationHandler<ModifiedEntityPushNotification>, ModifiedEntityPushNotificationHandler>();

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Logger.LogDebug("App is built");
// create routes for the identity endpoints

app.Logger.LogDebug("mapping IdentityApi");
app.MapGroup("/account").MapIdentityApi<ApplicationUser>( );

app.Logger.LogDebug("mapping Custom Registration");
BusinessLogicAPI.MapCustomRegistration(app);
app.Logger.LogDebug("mapping Test");
BusinessLogicAPI.MapTest(app);
app.Logger.LogDebug("mapping Ancillary");
BusinessLogicAPI.MapAnciliary(app);
app.Logger.LogDebug("mapping Farm");
BusinessLogicAPI.MapFarm(app);
app.Logger.LogDebug("mapping Joins");
BusinessLogicAPI.MapJoins(app);
app.Logger.LogDebug("mapping Livestock");
BusinessLogicAPI.MapLivestock(app);
app.Logger.LogDebug("mapping Scheduling");
BusinessLogicAPI.MapScheduling(app);
app.Logger.LogDebug("mapping SignalR Hub");
app.MapHub<NotificationHub>("/notificationhub");



// activate the CORS policy
app.Logger.LogDebug("activating CORS");
app.UseCors("wasm");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.Logger.LogDebug("Setting Redirect");
app.UseHttpsRedirection();
//if (app.Environment.IsDevelopment())
//{
//    app.Logger.LogDebug("Migrating DB");
//    bool migratingDb = false;
//    using (var scope = app.Services.CreateScope())
//    {
//        do
//        {
//            if (migratingDb) Task.Delay(1000).Wait();
//        } while (migratingDb);
//        migratingDb = true;
//        var services = scope.ServiceProvider;
//        var context = services.GetRequiredService<MicroAgManagementDbContext>();
//        context.Database.Migrate();
//    }
//    migratingDb = false;
//}
app.Logger.LogDebug("Running APP");
app.Run();
