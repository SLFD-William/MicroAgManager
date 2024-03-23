using Domain.Constants;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public class CreateLivestock : ICreateLivestock
    {
        public Guid CreatedBy { get; set; }
        public string CreationMode { get; set; }
        public LivestockModel Livestock { get; set; }
    }
    public static class LivestockLogic
    {
        public async static Task VerifyNoOpenBreedingRecord(DbContext genericContext, List<long> femaleId, Guid tenantId, CancellationToken cancellationToken)
        {
            var context = genericContext as IMicroAgManagementDbContext;
            if (context is null) return;
            var animal = await context.Livestocks.Include(b => b.Breed).ThenInclude(a => a.LivestockAnimal).FirstAsync(a=>a.Id==femaleId[0]);

            var existing = await context.BreedingRecords
            .Where(b => b.RecipientType==animal.Breed.LivestockAnimal.GetType().Name && 
                    b.RecipientTypeId== animal.Breed.LivestockAnimalId && 
                    femaleId.Contains(b.FemaleId) && b.TenantId == tenantId && !b.ResolutionDate.HasValue)
            .ToListAsync(cancellationToken);
            if (existing.Any())
            {
                var nameList = string.Join(", ", existing.Select(b => b.Female.Name).ToList()).Trim();
                throw new IncompleteBreedingRecordException(nameList, existing.First().Id, "Open Breeding Record exists. Please Resolve");
            }
        }
        public async static Task<List<ICreateLivestock>> OnBreedingRecordResolved(DbContext genericContext, long breedingRecordId)
        {
            var context = genericContext as IMicroAgManagementDbContext;
            var entitiesModified = new List<ICreateLivestock>();
            if (context is null) return entitiesModified;
            

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
                    CreationMode = "Birth",
                    Livestock = CreateBirthModel(i + 1, breedingRecord, female, status, GenderConstants.Female)
                });
            }
            for (int i = 0; i < breedingRecord.BornMales; i++)
            {
                entitiesModified.Add(new CreateLivestock
                {
                    CreatedBy = breedingRecord.ModifiedBy,
                    CreationMode = "Birth",
                    Livestock = CreateBirthModel(i + 1, breedingRecord, female, status, GenderConstants.Male)
                });
            }
            return entitiesModified;
        }
        public async static Task<List<ModifiedEntity>> OnLivestockBred(DbContext genericContext, long breedingRecordId, string source, long sourceId, CancellationToken cancellationToken)
        {
            var context = genericContext as IMicroAgManagementDbContext;
            var entitiesModified = new List<ModifiedEntity>();
            if (context is null) return entitiesModified;


            var breedingRecord = await context.BreedingRecords.FindAsync(breedingRecordId);
            if (breedingRecord == null) throw new Exception("Breeding Record not found");
            var livestock = await context.Livestocks.Include(b => b.Breed).ThenInclude(a => a.LivestockAnimal).FirstOrDefaultAsync(l => l.Id == breedingRecord.FemaleId);
            if (livestock == null || livestock.Gender == GenderConstants.Male) throw new Exception("Female Livestock not found");

            entitiesModified.Add(new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Modified", livestock.ModifiedBy, livestock.ModifiedOn));
            entitiesModified.Add(new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy, breedingRecord.ModifiedOn));

            var birthDuty = await context.Duties.FirstOrDefaultAsync(d => d.RecipientType == livestock.Breed.LivestockAnimal.GetType().Name && d.RecipientTypeId == livestock.Breed.LivestockAnimalId && d.Command == DutyCommandConstants.Birth);
            if (birthDuty == null) throw new Exception("Birth Duty not found");

            var scheduledDuty = new ScheduledDuty(breedingRecord.ModifiedBy, breedingRecord.TenantId)
            {
                DutyId=birthDuty.Id,
                DueOn = breedingRecord.ServiceDate.AddDays(livestock.Breed.GestationPeriod / 3),
                RecordId = breedingRecord.Id,
                Record = breedingRecord.GetType().Name,
                RecipientId = livestock.Id,
                Recipient = livestock.GetType().Name,
                ScheduleSource=source,
                ScheduleSourceId=sourceId
            };
            context.ScheduledDuties.Add(scheduledDuty);

            await context.SaveChangesAsync(cancellationToken);
            
            entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy, scheduledDuty.ModifiedOn));
            return entitiesModified;
        }
        public async static Task<List<ModifiedEntity>> OnLivestockBorn(DbContext genericContext, long livestockId, CancellationToken cancellationToken)
        {
            var context = genericContext as IMicroAgManagementDbContext;
            var entitiesModified = new List<ModifiedEntity>();
            if (context is null) return entitiesModified;


            var livestock = await context.Livestocks.Include(b => b.Breed).ThenInclude(a => a.LivestockAnimal).FirstOrDefaultAsync(l => l.Id == livestockId);
            if (livestock == null) throw new Exception("Livestock not found");
            entitiesModified.Add(new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Created", livestock.ModifiedBy, livestock.ModifiedOn));
            if (!livestock.StatusId.HasValue)
            { 
                var status = await context.LivestockStatuses.FirstOrDefaultAsync(l => l.LivestockAnimalId == livestock.Breed.LivestockAnimalId && l.DefaultStatus);
                livestock.StatusId = status?.Id;
                livestock.InMilk = status?.InMilk == LivestockStatusModeConstants.True;
                livestock.BeingManaged = status?.BeingManaged == LivestockStatusModeConstants.True;
                livestock.BottleFed = status?.BottleFed == LivestockStatusModeConstants.True;
                livestock.ForSale = status?.ForSale == LivestockStatusModeConstants.True;
                livestock.Sterile = status?.Sterile == LivestockStatusModeConstants.True;
            }
            var birthMilestoneDuties = await context.Milestones.Include(m => m.Duties).Where(m => m.Name == MilestoneSystemRequiredConstants.Birth).SelectMany(m => m.Duties).ToListAsync();
            foreach (var duty in birthMilestoneDuties.Where(d => (d.Gender is null || d.Gender == livestock.Gender) && d.Relationship == DutyRelationshipConstants.Self))
            {
                var record = DutyLogic.GetRecordTypeFromCommand(duty as Duty);
                if (await context.ScheduledDuties.AnyAsync(s => s.Duty == duty && s.RecipientId == livestock.Id
                        && s.Recipient == livestock.GetType().Name && !s.CompletedOn.HasValue))
                    continue;

                var scheduledDuty = new ScheduledDuty(livestock.ModifiedBy, livestock.TenantId)
                {
                    Duty = duty as Duty,
                    DueOn = livestock.Birthdate.AddDays(duty.DaysDue),
                    Record = record,
                    RecipientId = livestock.Id,
                    Recipient = livestock.GetType().Name,
                };
                context.ScheduledDuties.Add(scheduledDuty);
                entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy, scheduledDuty.ModifiedOn));
            }

            var mother = await context.Livestocks.FindAsync(livestock.MotherId);
            if (mother == null) throw new Exception("Livestock mother not found");
            entitiesModified.Add(new ModifiedEntity(mother.Id.ToString(), mother.GetType().Name, "Modified", mother.ModifiedBy, mother.ModifiedOn));
            var parturitionMilestoneDuties = await context.Milestones.Include(m => m.Duties).Where(m => m.Name == MilestoneSystemRequiredConstants.Parturition).SelectMany(m => m.Duties).ToListAsync();
            foreach (var duty in parturitionMilestoneDuties.Where(d => (d.Gender is null || d.Gender == mother.Gender) && d.Relationship == DutyRelationshipConstants.Self))
            {
                var record = DutyLogic.GetRecordTypeFromCommand(duty as Duty);
                if (await context.ScheduledDuties.AnyAsync(s => s.Duty == duty && s.RecipientId == mother.Id
                        && s.Recipient == mother.GetType().Name && !s.CompletedOn.HasValue))
                    continue;

                var scheduledDuty = new ScheduledDuty(livestock.ModifiedBy, livestock.TenantId)
                {
                    Duty = duty as Duty,
                    DueOn = livestock.Birthdate.AddDays(duty.DaysDue),
                    Record = record,
                    RecipientId = mother.Id,
                    Recipient = mother.GetType().Name,
                };
                context.ScheduledDuties.Add(scheduledDuty);
                entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy, scheduledDuty.ModifiedOn));
            }
            await context.SaveChangesAsync(cancellationToken);
            return entitiesModified;
        }
        private static LivestockModel CreateBirthModel(int count, BreedingRecord breedingRecord, Livestock female, LivestockStatus status, string gender)
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


















