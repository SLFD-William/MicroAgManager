using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logging;
using MediatR;
using MicroAgManager.API;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddDbContext<IMicroAgManagementDbContext, MicroAgManagementDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<MicroAgManagementDbContext>(options => options.UseSqlServer());

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
// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// create routes for the identity endpoints
app.MapGroup("/account").MapIdentityApi<ApplicationUser>( );


BusinessLogicAPI.MapCustomRegistration(app);
BusinessLogicAPI.MapTest(app);
BusinessLogicAPI.MapAnciliary(app);
BusinessLogicAPI.MapFarm(app);
BusinessLogicAPI.MapJoins(app);
BusinessLogicAPI.MapLivestock(app);
BusinessLogicAPI.MapScheduling(app);




// activate the CORS policy
app.UseCors("wasm");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
bool migratingDb = false;
using (var scope = app.Services.CreateScope())
{
    do
    {
        if (migratingDb) Task.Delay(1000).Wait();
    } while (migratingDb);
    migratingDb = true;
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MicroAgManagementDbContext>();
    context.Database.Migrate();
}
migratingDb = false;
app.Run();
