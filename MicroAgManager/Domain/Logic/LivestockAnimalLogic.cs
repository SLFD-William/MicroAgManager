using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class LivestockAnimalLogic
    {
        public async static Task<List<EntityPushNotification>> OnLivestockAnimalCreated(DbContext genericContext, long id, CancellationToken cancellationToken)
        {
            var context = genericContext as IMicroAgManagementDbContext;
            var entitiesModified = new List<EntityPushNotification>();
            if (context is null) return entitiesModified;

            var livestockAnimal = await context.LivestockAnimals.FindAsync(id);
                if (livestockAnimal == null) throw new Exception("LivestockAnimal not found");
                entitiesModified.Add(new EntityPushNotification(livestockAnimal.TenantId, LivestockAnimalModel.Create(livestockAnimal).GetJsonString(),nameof(LivestockAnimalModel)));

                AddRequiredMilestones(livestockAnimal, context);
                entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context as DbContext));
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
                var serviceDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
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
                var birthDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
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
                parturition = new Milestone(animalType.ModifiedBy, animalType.TenantId)
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
                death = new Milestone(animalType.ModifiedBy, animalType.TenantId)
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
                birth = new Milestone(animalType.ModifiedBy, animalType.TenantId)
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
                var breedDuty = new Duty(animalType.ModifiedBy, animalType.TenantId)
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
                breed = new Milestone(animalType.ModifiedBy, animalType.TenantId)
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
