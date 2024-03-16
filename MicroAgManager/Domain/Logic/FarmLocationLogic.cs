using Domain.Constants;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Logic
{
    public static class FarmLocationLogic
    {
        public async static Task<List<ModifiedEntity>> OnFarmLocationCreated(IMicroAgManagementDbContext context,long id, CancellationToken cancellationToken)
        { 
            var entitiesModified = new List<ModifiedEntity>();
            
            var farmLocation = await context.Farms.FindAsync(id);
            if (farmLocation == null) throw new Exception("FarmLocation not found");
            entitiesModified.Add(new ModifiedEntity(farmLocation.Id.ToString(), farmLocation.GetType().Name, "Created", farmLocation.ModifiedBy,farmLocation.ModifiedOn));
            AddAncilliaries(farmLocation, context);
            entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context));
            await context.SaveChangesAsync(cancellationToken);
            
            return entitiesModified;
        }
        private static void AddAncilliaries(Entity.FarmLocation farm, IMicroAgManagementDbContext context)
        {
            if (context.Units.Any(u => u.TenantId == farm.TenantId)) return;
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Acres",
                Category = UnitCategoryConstants.Area.Key,
                Symbol="acre",
                ConversionFactorToSIUnit= 4046.85642
            });
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Day",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "day",
                ConversionFactorToSIUnit = 86400
            });
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Hour",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "hour",
                ConversionFactorToSIUnit = 3600
            });
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Week",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "week",
                ConversionFactorToSIUnit = 604800
            });
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Month",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "month",
                ConversionFactorToSIUnit = 2592000
            });
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Year",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "year",
                ConversionFactorToSIUnit = 31536000
            });
            context.Units.Add(new Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Minute",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "minute",
                ConversionFactorToSIUnit = 60
            });
        }
    }
}
