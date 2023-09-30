using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.ValueObjects;

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
            entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context));
            if (save) await context.SaveChangesAsync(cancellationToken);
            return entitiesModified;
        }
        public static void AddRequiredMilestones(LivestockAnimal animalType, IMicroAgManagementDbContext context)
        {
            var parturition = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Parturition
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
                    CommandId = 0,
                    Command = DutyCommands.Birth
                };
                parturition = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockAnimal = animalType,
                    Name = MilestoneSystemRequiredConstants.Parturition,
                    Description= "The act of giving birth",
                };
                parturition.Duties.Add(birthDuty);
                context.Milestones.Add(parturition);
            }

            var death = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Death &&
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
                    Name = MilestoneSystemRequiredConstants.Death,
                    Description = "The act of dying",
                };
                context.Milestones.Add(death);
            }

            var birth = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Birth 
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
                    Name = MilestoneSystemRequiredConstants.Birth.ToString(),
                    Description = "The act of being born",
                };
                context.Milestones.Add(birth);
            }
            var breed = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Breed
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
                    Name = MilestoneSystemRequiredConstants.Breed,
                    CommandId = 0,
                    Command = DutyCommands.Breed,
                };
                breed = new Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    LivestockAnimal = animalType,
                    Name = MilestoneSystemRequiredConstants.Breed,
                    Description= "The act of breeding",
                };
                breed.Duties.Add(breedDuty);
                context.Milestones.Add(breed);
            }
        }
    }
}
