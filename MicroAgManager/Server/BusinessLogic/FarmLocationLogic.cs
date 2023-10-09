using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logic;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.BusinessLogic
{
    public static class FarmLocationLogic
    {
        public async static Task<List<ModifiedEntity>> OnFarmLocationCreated(IMicroAgManagementDbContext context, long id, CancellationToken cancellationToken, bool save = true)
        { 
            var entitiesModified = new List<ModifiedEntity>();
            var farmLocation = await context.Farms.FindAsync(id);
            if (farmLocation == null) throw new Exception("FarmLocation not found");
            entitiesModified.Add(new ModifiedEntity(farmLocation.Id.ToString(), farmLocation.GetType().Name, "Created", farmLocation.ModifiedBy));
            AddAncilliaries(farmLocation, context);
            entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context));
            if (save) await context.SaveChangesAsync(cancellationToken);
            return entitiesModified;
        }
        private static void AddAncilliaries(Domain.Entity.FarmLocation farm, IMicroAgManagementDbContext context)
        {
            if (context.Units.Any(u => u.TenantId == farm.TenantId)) return;
            var unit = new Domain.Entity.Unit(farm.ModifiedBy, farm.TenantId)
            {
                Name = "Acres",
                Category = UnitCategoryConstants.Area.Key,
                Symbol="acre",
                ConversionFactorToSIUnit= 4046.85642
            };
            context.Units.Add(unit);
        }
    }
}
