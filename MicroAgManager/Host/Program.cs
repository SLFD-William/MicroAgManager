using BackEnd.Infrastructure;
using Host;
using Host.Hubs;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Persistence;

const string CorsPolicy = nameof(CorsPolicy);
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddBackEndMediatR();
builder.Services.AddBackEndPersistenceAndLogging(builder.Configuration);
builder.Services.AddBackEndAuthentication(builder.Configuration);


builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: CorsPolicy,
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; })
               .AddMessagePackProtocol();
builder.Services.AddScoped<INotificationHandler<EntitiesModifiedNotification>, NotificationHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseCors(CorsPolicy);
app.UseAuthorization();
AuthenticationAPI.Map(app);
BusinessLogicAPI.MapAnciliary(app);
BusinessLogicAPI.MapFarm(app);
BusinessLogicAPI.MapJoins(app);
BusinessLogicAPI.MapLivestock(app);
BusinessLogicAPI.MapScheduling(app);

app.MapHub<NotificationHub>("/notificationhub");
app.MapFallbackToFile("index.html");
bool migratingDb = false;
using (var scope = app.Services.CreateScope())
{
    do
    {
       if(migratingDb) Task.Delay(1000).Wait();
    } while (migratingDb);
    migratingDb = true;
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MicroAgManagementDbContext>();
    context.Database.Migrate();
}
migratingDb = false;
app.Run();
