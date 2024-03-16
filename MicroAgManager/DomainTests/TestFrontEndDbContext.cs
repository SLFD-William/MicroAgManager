using Domain.Constants;
using Domain.Interfaces;
using Domain.Models;
using FrontEnd.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

public class TestFrontEndDbContext : IDisposable
{
    private static DbConnection _dbConnection;
    private static DbContextOptions<FrontEndDbContext> _dbContextOptions;
    private static FrontEndDbContext _dbContext;
    public void Dispose() => _dbConnection.Dispose();

    public IFrontEndDbContext CreateContext()
    {
        _dbConnection = new SqliteConnection("Data Source=Server;Mode=Memory;");
        _dbConnection.Open();
        _dbContextOptions = new DbContextOptionsBuilder<FrontEndDbContext>()
            .UseSqlite(_dbConnection)
            .EnableSensitiveDataLogging()
            .Options;

        // Create the schema and seed some data
        _dbContext = new FrontEndDbContext(_dbContextOptions);
        if (!_dbContext.Database.EnsureCreated())
            throw new Exception("Fake Server DB Context not Created");

        SeedTesting(_dbContext);

        return _dbContext;
    }
    private static void SeedTesting(IFrontEndDbContext context)
    {
        SeedUnits(context);
        SeedPlots(context);
        SeedLivestock(context);
        SeedScheduling(context);
        context.SaveChanges();
    }
    private static void SeedUnits(IFrontEndDbContext context)
    {
        context.Units.Add(new UnitModel
        {
            Id = 1,
            Name = "Day",
            Category = UnitCategoryConstants.Time.Key,
            Symbol = "day",
            ConversionFactorToSIUnit = 86400,
        });
        context.Units.Add(new UnitModel
        {
            Id = 2,
            Name = "Acres",
            Category = UnitCategoryConstants.Area.Key,
            Symbol = "acre",
            ConversionFactorToSIUnit = 4046.85642
        });
        context.Units.Add(new UnitModel
        {
            Id = 3,
            Name = "Hour",
            Category = UnitCategoryConstants.Time.Key,
            Symbol = "hour",
            ConversionFactorToSIUnit = 3600
        });
        context.Units.Add(new UnitModel
        {
            Id = 4,
            Name = "Week",
            Category = UnitCategoryConstants.Time.Key,
            Symbol = "week",
            ConversionFactorToSIUnit = 604800
        });
        context.Units.Add(new UnitModel
        {
            Id = 5,
            Name = "Month",
            Category = UnitCategoryConstants.Time.Key,
            Symbol = "month",
            ConversionFactorToSIUnit = 2592000
        });
        context.Units.Add(new UnitModel
        {
            Id = 6,
            Name = "Year",
            Category = UnitCategoryConstants.Time.Key,
            Symbol = "year",
            ConversionFactorToSIUnit = 31536000
        });
        context.Units.Add(new UnitModel
        {
            Id = 7,
            Name = "Minute",
            Category = UnitCategoryConstants.Time.Key,
            Symbol = "minute",
            ConversionFactorToSIUnit = 60
        });

    }
    private static void SeedPlots(IFrontEndDbContext context) {
        context.Farms.Add(new FarmLocationModel
        {
            Id = 1,
            Name = "Test Farm",
            TenantId = new Guid(),
            StreetAddress = "Test Address",
            City = "Test City",
            State = "Test State",
            Zip = "55555"
        });
        context.LandPlots.Add(new LandPlotModel
        {
            Name = "Test Plot",
            Description = "Testing Plot",
            AreaUnitId = 2,
            Id = 1,
            FarmLocationId = 1,
        });
    }
    private static void SeedLivestock(IFrontEndDbContext context)
    {
        context.LivestockAnimals.Add(new LivestockAnimalModel
        {
            Id = 1,
            Name = "Test Animal",
            GroupName = "Test Group",
            ParentFemaleName = "Test Mom",
            ParentMaleName = "Test Dad",
            Care = LivestockCareConstants.Individual
        });
        context.LivestockStatuses.Add(new LivestockStatusModel
        {
            Id = 1,
            LivestockAnimalId = 1,
            Status = "Test Status",
            DefaultStatus = false,
            BeingManaged = LivestockStatusModeConstants.Unchanged,
            BottleFed = LivestockStatusModeConstants.Unchanged,
            ForSale = LivestockStatusModeConstants.Unchanged,
            InMilk = LivestockStatusModeConstants.Unchanged,
            Sterile = LivestockStatusModeConstants.Unchanged
        });
        context.LivestockBreeds.Add(new LivestockBreedModel
        {
            Id = 1,
            LivestockAnimalId = 1,
            GestationPeriod = 1,
            HeatPeriod = 1,
            Name = "Test Breed",
            EmojiChar = "TE"
        });
        context.Livestocks.Add(new LivestockModel
        {
            Id = 1,
            Name = "Test Livestock",
            LivestockBreedId = 1,
            StatusId = 1,
            LocationId = 1,
            BatchNumber = "Test Batch",
            Birthdate = DateTime.Now,
            Gender = "F",
            Variety = "Test Variety",
            Description = "Test Description"
        });

    }
    private static void SeedScheduling(IFrontEndDbContext context)
    {
        context.Chores.Add(new ChoreModel
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

    }

}
