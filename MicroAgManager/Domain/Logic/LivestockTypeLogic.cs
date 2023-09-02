using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class LivestockAnimalLogic
    {
        public async static Task<List<ModifiedEntity>> OnLivestockAnimalCreated(IMicroAgManagementDbContext context,long id, CancellationToken cancellationToken, bool save = true)
        {
            var entitiesModified= new List<ModifiedEntity>();
            var LivestockAnimal = await context.LivestockAnimals.FindAsync(id);
            if (LivestockAnimal == null) throw new Exception("LivestockAnimal not found");
            entitiesModified.Add(new ModifiedEntity(LivestockAnimal.Id.ToString(), LivestockAnimal.GetType().Name, "Created",LivestockAnimal.ModifiedBy));

            AddRequiredMilestones(LivestockAnimal, context);
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
        public static void AddRequiredMilestones(LivestockAnimal animalType, IMicroAgManagementDbContext context)
        {
            var parturition = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Parturition
                && e.TenantId == animalType.TenantId
                && e.LivestockAnimal !=null 
                && e.LivestockAnimal.Id == animalType.Id);
            if (parturition == null)
            {
                var birthDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Female,
                    LivestockAnimal = animalType,
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
                    LivestockAnimal = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Parturition,
                };
                parturition.Duties.Add(birthDuty);
                
                context.Milestones.Add(parturition);
            }

            var death = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Death &&
                e.TenantId == animalType.TenantId
                && e.LivestockAnimal != null
                && e.LivestockAnimal.Id == animalType.Id);
            if (death == null)
            {
                death = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockAnimal = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Death
                };
                context.Milestones.Add(death);
            }

            var birth = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Birth 
                && e.TenantId == animalType.TenantId 
                && e.LivestockAnimal != null 
                && e.LivestockAnimal.Id == animalType.Id);
            if (birth == null)
            {
                birth = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockAnimal = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Birth.ToString()
                };
                context.Milestones.Add(birth);
            }
            var breed = context.Milestones.FirstOrDefault(e => e.Subcategory == MilestoneSubcategorySystemRequiredConstants.Breed
                && e.TenantId == animalType.TenantId
                && e.LivestockAnimal != null
                && e.LivestockAnimal.Id == animalType.Id);
            if (breed == null)
            {
                var breedDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Female,
                    LivestockAnimal = animalType,
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
                    LivestockAnimal = animalType,
                    Subcategory = MilestoneSubcategorySystemRequiredConstants.Breed
                };
                breed.Duties.Add(breedDuty);
                context.Milestones.Add(breed);
            }
        }
    }
}
