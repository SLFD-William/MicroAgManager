using Domain.Constants;
using Domain.Interfaces;
using Domain.Logic;
using Domain.ValueObjects;

namespace BackEnd.BusinessLogic
{
    public static class FarmLocationLogic
    {
        public async static Task<List<ModifiedEntity>> OnFarmLocationCreated(IMicroAgManagementDbContext context,long id, CancellationToken cancellationToken)
        { 
            var entitiesModified = new List<ModifiedEntity>();
            
            var farmLocation = await context.Farms.FindAsync(id);
            if (farmLocation == null) throw new Exception("FarmLocation not found");
            entitiesModified.Add(new ModifiedEntity(farmLocation.Id.ToString(), farmLocation.GetType().Name, "Created", farmLocation.ModifiedBy));
            AddAncilliaries(farmLocation, context);
            entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context));
            await context.SaveChangesAsync(cancellationToken);
            
            return entitiesModified;
        }
        private static void AddAncilliaries(Domain.Entity.FarmLocation farm, IMicroAgManagementDbContext context)
        {
            if (context.Units.Any(u => u.TenantId == farm.TenantId)) return;
            context.Units.Add(new Domain.Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Acres",
                Category = UnitCategoryConstants.Area.Key,
                Symbol="acre",
                ConversionFactorToSIUnit= 4046.85642
            });
            context.Units.Add(new Domain.Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Daily",
                Category = UnitCategoryConstants.Frequency.Key,
                Symbol = "daily",
                ConversionFactorToSIUnit = 1/86400
            });
            context.Units.Add(new Domain.Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Day",
                Category = UnitCategoryConstants.Time.Key,
                Symbol = "day",
                ConversionFactorToSIUnit = 86400
            });
        }
    }
}
