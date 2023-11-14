using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackEnd
{
    internal class DbContextFactory : IDbContextFactory<BackEndDbContext>
    {
        private const string ConnectionStringName = "DefaultConnection";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        public BackEndDbContext CreateDbContext()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
               .SetBasePath(basePath)
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.Local.json", optional: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(AspNetCoreEnvironment)}.json", optional: true)
               .AddEnvironmentVariables()
               .Build();

            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            var optionsBuilder = new DbContextOptionsBuilder<BackEndDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(ConnectionStringName));

            return new BackEndDbContext(optionsBuilder.Options);
        }
    }
}
