using BackEnd.BusinessLogic.Livestock;
using Domain.Constants;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic
{
    public static class LivestockLogic
    {
        public async static Task VerifyNoOpenBreedingRecord(IMicroAgManagementDbContext context,List<long> femaleId, Guid tenantId, CancellationToken cancellationToken)
        {
            var existing = await context.BreedingRecords
            .Where(b => femaleId.Contains(b.FemaleId) && b.TenantId == tenantId && !b.ResolutionDate.HasValue)
            .ToListAsync(cancellationToken);
            if (existing.Any())
            {
                var nameList = string.Join(", ", existing.Select(b => b.Female.Name).ToList()).Trim();
                throw new IncompleteBreedingRecordException(nameList, existing.First().Id, "Open Breeding Record exists. Please Resolve");
            }
        }
        public async static Task<List<CreateLivestock>> OnBreedingRecordResolved(IMicroAgManagementDbContext context, long breedingRecordId)
        {
            var entitiesModified = new List<CreateLivestock>();

            var breedingRecord = await context.BreedingRecords.FindAsync(breedingRecordId);
            if (breedingRecord == null) throw new Exception("Breeding Record not found");
            if (breedingRecord.Resolution != BreedingResolutionConstants.Success || !breedingRecord.ResolutionDate.HasValue) return entitiesModified;
            var female = await context.Livestocks.Include(b => b.Breed).FirstOrDefaultAsync(l => l.Id == breedingRecord.FemaleId);
            var status = await context.LivestockStatuses.FirstOrDefaultAsync(l => l.LivestockAnimalId == female.Breed.LivestockAnimalId && l.DefaultStatus);
            if (status is null)
                status = await context.LivestockStatuses.FirstOrDefaultAsync(l => l.LivestockAnimalId == female.Breed.LivestockAnimalId);
            for (int i = 0; i < breedingRecord.BornFemales; i++)
            {
                entitiesModified.Add(new CreateLivestock
                {
                    CreatedBy = breedingRecord.ModifiedBy,
                    TenantId = breedingRecord.TenantId,
                    CreationMode = "Birth",
                    Livestock = CreateBirthModel(i + 1, breedingRecord, female, status, GenderConstants.Female)
                });
            }
            for (int i = 0; i < breedingRecord.BornMales; i++)
            {
                entitiesModified.Add(new CreateLivestock
                {
                    CreatedBy = breedingRecord.ModifiedBy,
                    TenantId = breedingRecord.TenantId,
                    CreationMode = "Birth",
                    Livestock = CreateBirthModel(i + 1, breedingRecord, female, status, GenderConstants.Male)
                });
            }
            return entitiesModified;
        }
        public async static Task<List<ModifiedEntity>> OnLivestockBred(IMicroAgManagementDbContext context, long breedingRecordId, CancellationToken cancellationToken)
        {
            var entitiesModified = new List<ModifiedEntity>();

            var breedingRecord = await context.BreedingRecords.FindAsync(breedingRecordId);
            if (breedingRecord == null) throw new Exception("Breeding Record not found");
            var livestock = await context.Livestocks.Include(b => b.Breed).ThenInclude(a => a.LivestockAnimal).FirstOrDefaultAsync(l => l.Id == breedingRecord.FemaleId);
            if (livestock == null || livestock.Gender == GenderConstants.Male) throw new Exception("Female Livestock not found");

            entitiesModified.Add(new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Modified", breedingRecord.ModifiedBy));
            entitiesModified.Add(new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy));

            var birthDuty = await context.Duties.FirstOrDefaultAsync(d => d.RecipientType == livestock.Breed.LivestockAnimal.GetType().Name && d.RecipientTypeId == livestock.Breed.LivestockAnimalId && d.Command == DutyCommandConstants.Birth);
            if (birthDuty == null) throw new Exception("Birth Duty not found");

            var scheduledDuty = new Domain.Entity.ScheduledDuty(breedingRecord.ModifiedBy, breedingRecord.TenantId)
            {
                Duty = birthDuty,
                DueOn = breedingRecord.ServiceDate.AddDays(livestock.Breed.GestationPeriod / 3),
                RecordId = breedingRecord.Id,
                Record = breedingRecord.GetType().Name,
                RecipientId = livestock.Id,
                Recipient = livestock.GetType().Name,
            };
            context.ScheduledDuties.Add(scheduledDuty);

            await context.SaveChangesAsync(cancellationToken);
            
            entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy));
            return entitiesModified;
        }
        public async static Task<List<ModifiedEntity>> OnLivestockBorn(IMicroAgManagementDbContext context, long livestockId, CancellationToken cancellationToken)
        {
            var entitiesModified = new List<ModifiedEntity>();

                var livestock = await context.Livestocks.Include(b => b.Breed).ThenInclude(a => a.LivestockAnimal).FirstOrDefaultAsync(l => l.Id == livestockId);
                if (livestock == null) throw new Exception("Livestock not found");

                var birthMilestoneDuties = await context.Milestones.Include(m => m.Duties).Where(m => m.Name == MilestoneSystemRequiredConstants.Birth).SelectMany(m => m.Duties).ToListAsync();
                foreach (var duty in birthMilestoneDuties.Where(d => (d.Gender is null || d.Gender == livestock.Gender) && d.Relationship == DutyRelationshipConstants.Self))
                {
                    var record = DutyLogic.GetRecordTypeFromCommand(duty);
                    if (await context.ScheduledDuties.AnyAsync(s => s.Duty == duty && s.RecipientId == livestock.Id
                            && s.Recipient == livestock.GetType().Name && !s.CompletedOn.HasValue))
                        continue;

                    var scheduledDuty = new Domain.Entity.ScheduledDuty(livestock.ModifiedBy, livestock.TenantId)
                    {
                        Duty = duty,
                        DueOn = livestock.Birthdate.AddDays(duty.DaysDue),
                        Record = record,
                        RecipientId = livestock.Id,
                        Recipient = livestock.GetType().Name,
                    };
                    context.ScheduledDuties.Add(scheduledDuty);
                    entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy));
                }

                var mother = await context.Livestocks.FindAsync(livestock.MotherId);
                if (mother == null) throw new Exception("Livestock mother not found");
                var parturitionMilestoneDuties = await context.Milestones.Include(m => m.Duties).Where(m => m.Name == MilestoneSystemRequiredConstants.Parturition).SelectMany(m => m.Duties).ToListAsync();
                foreach (var duty in parturitionMilestoneDuties.Where(d => (d.Gender is null || d.Gender == mother.Gender) && d.Relationship == DutyRelationshipConstants.Self))
                {
                    var record = DutyLogic.GetRecordTypeFromCommand(duty);
                    if (await context.ScheduledDuties.AnyAsync(s => s.Duty == duty && s.RecipientId == mother.Id
                            && s.Recipient == mother.GetType().Name && !s.CompletedOn.HasValue))
                        continue;

                    var scheduledDuty = new Domain.Entity.ScheduledDuty(livestock.ModifiedBy, livestock.TenantId)
                    {
                        Duty = duty,
                        DueOn = livestock.Birthdate.AddDays(duty.DaysDue),
                        Record = record,
                        RecipientId = mother.Id,
                        Recipient = mother.GetType().Name,
                    };
                    context.ScheduledDuties.Add(scheduledDuty);
                    entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy));
                }
                await context.SaveChangesAsync(cancellationToken);
            return entitiesModified;
        }
        private static LivestockModel CreateBirthModel(int count, Domain.Entity.BreedingRecord breedingRecord, Domain.Entity.Livestock female, LivestockStatus status, string gender)
                => new LivestockModel
                {
                    BatchNumber = breedingRecord.ResolutionDate.Value.ToString("yyyyMMdd"),
                    Gender = gender,
                    MotherId = female.Id,
                    FatherId = breedingRecord.MaleId,
                    StatusId = status.Id,
                    LocationId = female.LocationId,
                    LivestockBreedId = female.LivestockBreedId,
                    Birthdate = breedingRecord.ResolutionDate.Value,
                    Name = $"{female.Name} {gender} {count}",
                    BeingManaged = status.BeingManaged == LivestockStatusModeConstants.True,
                    InMilk = status.InMilk == LivestockStatusModeConstants.True,
                    BottleFed = status.BottleFed == LivestockStatusModeConstants.True,
                    ForSale = status.ForSale == LivestockStatusModeConstants.True,
                    Sterile = status.Sterile == LivestockStatusModeConstants.True,
                    Variety = female.Variety,
                    Description = string.Empty
                };
    }
}


















