using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Data.Common;

namespace DomainTests
{
    public class TestMicroAgManagementDbContext : IDisposable
    {
        private DbConnection? _dbConnection;
        private static DbContextOptions<MicroAgManagementDbContext>? _dbContextOptions;
        private MicroAgManagementDbContext? _dbContext;
        public void Dispose() => _dbConnection?.Dispose();
        public IMicroAgManagementDbContext CreateContext()
        {
            _dbConnection = new SqliteConnection("Data Source=Server;Mode=Memory;");
            _dbConnection.Open();
            _dbContextOptions = new DbContextOptionsBuilder<MicroAgManagementDbContext>()
                .UseSqlite(_dbConnection)
                .EnableSensitiveDataLogging()
                .Options;
            _dbContext = new MicroAgManagementDbContext(_dbContextOptions);
            if (!_dbContext.Database.EnsureCreated())
                throw new Exception("Fake Server DB Context not Created");

            SeedTesting(_dbContext);

            return _dbContext;

        }
        private static void SeedTesting(IMicroAgManagementDbContext context)
        {
            var Id=Guid.NewGuid();
            SeedUnits(context,Id);
            //SeedPlots(context);
            //SeedLivestock(context);
            SeedScheduling(context, Id);
            context.SaveChanges();
        }
        private static void SeedScheduling(IMicroAgManagementDbContext context,Guid TenantUserId)
        {
            context.Chores.Add(new Chore(TenantUserId, TenantUserId)
            {
                RecipientTypeId = 1,
                RecipientType = "LivestockAnimal",
                Name = "Test Chore",
                Color = "transparent",
                Id = 1,
                DueByTime = TimeSpan.FromHours(9),
                PerScalar = 1,
                PerUnitId = 1,
                EveryScalar = 1,
                EveryUnitId = 1
            });
            context.Duties.Add(new Duty(TenantUserId, TenantUserId)
            {
                Id = 1,
                Name = "Test Duty",
                DaysDue = 0,
                Command = "Test Command",
                CommandId = 0,
                RecipientType = "Test Recipient Type",
                RecipientTypeId = 0,
                Relationship = "Test Relationship",
                SystemRequired = false,
            });
            context.ScheduledDuties.Add(new ScheduledDuty(TenantUserId, TenantUserId)
            {
                Id = 1,
                DutyId = 1,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                ScheduleSourceId = 1,
                Recipient = "Livestock",
                RecipientId = 1,
                Record = "None"
            });

        }
        private static void SeedUnits(IMicroAgManagementDbContext context, Guid TenantUserId)
        {
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 1,
                Name = "Day",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "day",
                ConversionFactorToSIUnit = 86400,
            });
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 2,
                Name = "Acres",
                Category = UnitCategoryConstants.Area.Key,
                Symbol = "acre",
                ConversionFactorToSIUnit = 4046.85642
            });
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 3,
                Name = "Hour",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "hour",
                ConversionFactorToSIUnit = 3600
            });
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 4,
                Name = "Week",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "week",
                ConversionFactorToSIUnit = 604800
            });
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 5,
                Name = "Month",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "month",
                ConversionFactorToSIUnit = 2592000
            });
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 6,
                Name = "Year",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "year",
                ConversionFactorToSIUnit = 31536000
            });
            context.Units.Add(new Unit(TenantUserId, TenantUserId)
            {
                Id = 7,
                Name = "Minute",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "minute",
                ConversionFactorToSIUnit = 60
            });

        }
    }
}
