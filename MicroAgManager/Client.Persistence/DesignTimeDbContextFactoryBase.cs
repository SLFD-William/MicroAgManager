using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Persistence
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> :
          IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private const string ConnectionStringName = "DefaultConnection";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public TContext CreateDbContext(string[] args) => Create();


        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);



        private TContext Create()
        {

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseSqlite($"Filename=app.db");
            optionsBuilder.EnableSensitiveDataLogging(true);

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}
