using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;

namespace Domain.Logic
{
    public static class LivestockTypeLogic
    {
        public async static Task OnLivestockTypeCreated(IMicroAgManagementDbContext context,long id, CancellationToken cancellationToken, bool save = true)
        {
            var livestockType = await context.LivestockTypes.FindAsync(id);
            if (livestockType == null) throw new Exception("LivestockType not found");
            AddLivestockTypeStatus(livestockType, context);

            if (save) await context.SaveChangesAsync(cancellationToken);
        }
        public static void AddLivestockTypeStatus(LivestockType animalType, IMicroAgManagementDbContext context)
        {
            var animalStatusExists = context.LivestockStatuses.Any(_ => _.TenantId == animalType.TenantId && _.Status == animalType.DefaultStatus && _.LivestockType == animalType);
            if (animalStatusExists)
                return;

            var animalStatus = new LivestockStatus(animalType.ModifiedBy, animalType.TenantId)
            {
                ModifiedBy = animalType.ModifiedBy,
                ModifiedOn = animalType.ModifiedOn,
                TenantId = animalType.TenantId,
                LivestockType = animalType,
                Status = animalType.DefaultStatus,
                BeingManaged = LivestockStatusModeConstants.True,
                BottleFed = LivestockStatusModeConstants.Unchanged,
                ForSale = LivestockStatusModeConstants.Unchanged,
                InMilk = LivestockStatusModeConstants.Unchanged,
                Sterile = LivestockStatusModeConstants.Unchanged
            };
            context.LivestockStatuses.Add(animalStatus);
        }
        public static void AddRequiredMilestones(LivestockType animalType, IMicroAgManagementDbContext context)
        { }
    }
}
