using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logic;
using Domain.ValueObjects;

namespace BackEnd.BusinessLogic
{
    public static class LivestockAnimalLogic
    {
        public async static Task<List<ModifiedEntity>> OnLivestockAnimalCreated(IMicroAgManagementDbContext context,long id, CancellationToken cancellationToken)
        {
            var entitiesModified= new List<ModifiedEntity>();

                var LivestockAnimal = await context.LivestockAnimals.FindAsync(id);
                if (LivestockAnimal == null) throw new Exception("LivestockAnimal not found");
                entitiesModified.Add(new ModifiedEntity(LivestockAnimal.Id.ToString(), LivestockAnimal.GetType().Name, "Created", LivestockAnimal.ModifiedBy));

                AddRequiredMilestones(LivestockAnimal, context);
                entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context));
                await context.SaveChangesAsync(cancellationToken);

            return entitiesModified;
        }
        private static void AddRequiredMilestones(LivestockAnimal animalType, IMicroAgManagementDbContext context)
        {
            var parturition = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Parturition
                && e.TenantId == animalType.TenantId
                && e.RecipientType ==animalType.GetType().Name
                && e.RecipientTypeId == animalType.Id);
            if (parturition == null)
            {
                var serviceDuty = new Domain.Entity.Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Male,
                    RecipientType = animalType.GetType().Name,
                    RecipientTypeId = animalType.Id,
                    DaysDue = 0,
                    Relationship = DutyRelationshipConstants.Self,
                    SystemRequired = true,
                    Name = $"Service {animalType.Name}",
                    CommandId = 0,
                    Command = DutyCommandConstants.Service
                };
                context.Duties.Add(serviceDuty);
                var birthDuty = new Domain.Entity.Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Female,
                    RecipientType=animalType.GetType().Name,
                    RecipientTypeId=animalType.Id,
                    DaysDue = 0,
                    Relationship = DutyRelationshipConstants.Self,
                    SystemRequired = true,
                    Name = $"Birth {animalType.Name}",
                    CommandId = 0,
                    Command = DutyCommandConstants.Birth
                };
                parturition = new Domain.Entity.Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    RecipientType = animalType.GetType().Name,
                    RecipientTypeId = animalType.Id,
                    Name = $"{MilestoneSystemRequiredConstants.Parturition} {animalType.Name}",
                    Description= "The act of giving birth",
                };
                parturition.Duties.Add(birthDuty);
                context.Milestones.Add(parturition);
            }

            var death = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Death &&
                e.TenantId == animalType.TenantId
                && e.RecipientType == animalType.GetType().Name
                && e.RecipientTypeId == animalType.Id);
            if (death == null)
            {
                death = new Domain.Entity.Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    RecipientType = animalType.GetType().Name,
                    RecipientTypeId = animalType.Id,
                    Name = $"{MilestoneSystemRequiredConstants.Death} {animalType.Name}",
                    Description = "The act of dying",
                };
                context.Milestones.Add(death);
            }

            var birth = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Birth 
                && e.TenantId == animalType.TenantId
                && e.RecipientType == animalType.GetType().Name
                && e.RecipientTypeId == animalType.Id);
            if (birth == null)
            {
                birth = new Domain.Entity.Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    RecipientType = animalType.GetType().Name,
                    RecipientTypeId = animalType.Id,
                    Name = $"{MilestoneSystemRequiredConstants.Birth} {animalType.Name}",
                    Description = "The act of being born",
                };
                context.Milestones.Add(birth);
            }
            var breed = context.Milestones.FirstOrDefault(e => e.Name == MilestoneSystemRequiredConstants.Breed
                && e.TenantId == animalType.TenantId
                && e.RecipientType == animalType.GetType().Name
                && e.RecipientTypeId == animalType.Id);
            if (breed == null)
            {
                var breedDuty = new Domain.Entity.Duty(animalType.ModifiedBy, animalType.TenantId)
                {
                    Gender = GenderConstants.Female,
                    RecipientType = animalType.GetType().Name,
                    RecipientTypeId = animalType.Id,
                    DaysDue = 0,
                    Relationship = DutyRelationshipConstants.Self,
                    SystemRequired = true,
                    Name = $"{MilestoneSystemRequiredConstants.Breed} {animalType.Name}",
                    CommandId = 0,
                    Command = DutyCommandConstants.Breed,
                };
                breed = new Domain.Entity.Milestone(animalType.ModifiedBy, animalType.TenantId)
                {
                    SystemRequired = true,
                    ModifiedBy = animalType.ModifiedBy,
                    ModifiedOn = animalType.ModifiedOn,
                    TenantId = animalType.TenantId,
                    RecipientType = animalType.GetType().Name,
                    RecipientTypeId = animalType.Id,
                    Name = $"{MilestoneSystemRequiredConstants.Breed} {animalType.Name}",
                    Description = "The act of breeding",
                };
                breed.Duties.Add(breedDuty);
                context.Milestones.Add(breed);
            }
        }
    }
}
