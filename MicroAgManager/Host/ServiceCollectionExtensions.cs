using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace Host
{
    static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddBackEndPersistenceAndLogging(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<IMicroAgManagementDbContext, MicroAgManagementDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<MicroAgManagementDbContext>();

            //add the DatabaseLoggingProvider
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddProvider(new DatabaseLoggingProvider(services.BuildServiceProvider().GetService<IMicroAgManagementDbContext>(), configuration));
            });
            services.AddSingleton(typeof(ILogger), typeof(Logger<Log>));


            return services;
        }
        internal static IServiceCollection AddBackEndAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            var config = configuration["Jwt:Secret"] ?? string.Empty;
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config))
                };
            });

            return services;
        }

        internal static IServiceCollection AddBackEndMediatR(
this IServiceCollection services)
        {
            //// Add MediatR
            ///
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
//            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

            return services;
        }
    }
}
