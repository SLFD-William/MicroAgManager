using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class LivestockTypeLogic
    {
        public async static Task<List<ModifiedEntity>> OnLivestockTypeCreated(IMicroAgManagementDbContext context,long id, CancellationToken cancellationToken, bool save = true)
        {
            var entitiesModified= new List<ModifiedEntity>();
            var livestockType = await context.LivestockTypes.FindAsync(id);
            if (livestockType == null) throw new Exception("LivestockType not found");
            entitiesModified.Add(new ModifiedEntity(livestockType.Id.ToString(), livestockType.GetType().Name, "Created",livestockType.ModifiedBy));

            AddRequiredMilestones(livestockType, context);
            var changes = ((DbContext)context).ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();
            foreach (var change in changes)
            {
                if(change.Entity as BaseEntity is null) continue;
                var modification=change.State == EntityState.Added ? "Created" : "Modified";
                var entityName = change.Entity.GetType().Name;
                var entityId = change.Entity.GetType().GetProperty("Id").GetValue(change.Entity);
                var modifiedBy = change.Entity.GetType().GetProperty("ModifiedBy").GetValue(change.Entity);
                entitiesModified.Add(new ModifiedEntity(entityId.ToString(), entityName,modification, (Guid)modifiedBy));
            }
            if (save) await context.SaveChangesAsync(cancellationToken);
            return entitiesModified;
        }
        public static void AddRequiredMilestones(LivestockType animalType, IMicroAgManagementDbContext context)
        {
            var parturition = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Parturition
                && e.TenantId == animalType.TenantId
                && e.LivestockType !=null 
                && e.LivestockType.Id == animalType.Id);
            if (parturition == null)
            {
                var birthDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Female,
                    LivestockType = animalType,
                    DaysDue = 0,
                    Relationship = DutyRelationshipConstants.Self,
                    SystemRequired = true,
                    Name = "Birth",
                    DutyTypeId = 0,
                    DutyType = DutyTypeConstants.Birth
                };
                parturition = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockType = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Parturition,
                };
                parturition.Duties.Add(birthDuty);
                
                context.Milestones.Add(parturition);
            }

            var death = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Death &&
                e.TenantId == animalType.TenantId
                && e.LivestockType != null
                && e.LivestockType.Id == animalType.Id);
            if (death == null)
            {
                death = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockType = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Death
                };
                context.Milestones.Add(death);
            }

            var birth = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Birth 
                && e.TenantId == animalType.TenantId 
                && e.LivestockType != null 
                && e.LivestockType.Id == animalType.Id);
            if (birth == null)
            {
                birth = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockType = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Birth.ToString()
                };
                context.Milestones.Add(birth);
            }
            var breed = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Breed
                && e.TenantId == animalType.TenantId
                && e.LivestockType != null
                && e.LivestockType.Id == animalType.Id);
            if (breed == null)
            {
                var breedDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Female,
                    LivestockType = animalType,
                    DaysDue = 0,
                    Relationship = DutyRelationshipConstants.Self,
                    SystemRequired = true,
                    Name = MilestoneSubcategorySystemRequiredConstants.Breed,
                    DutyTypeId = 0,
                    DutyType = DutyTypeConstants.Breed,
                };
                breed = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockType = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Breed
                };
                breed.Duties.Add(breedDuty);
                context.Milestones.Add(breed);
            }
        }
    }
}
