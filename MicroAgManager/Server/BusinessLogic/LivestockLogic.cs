using BackEnd.BusinessLogic.Livestock;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic
{
    public static class LivestockLogic
    {
        public async static Task VerifyNoOpenBreedingRecord(List<long> femaleId, Guid tenantId, IMicroAgManagementDbContext context, CancellationToken cancellationToken)
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
        public async static Task<List<ModifiedEntity>> OnLivestockBred(IMicroAgManagementDbContext context, long breedingRecordId, CancellationToken cancellationToken, bool save = true)
        {
            var entitiesModified = new List<ModifiedEntity>();
            var breedingRecord = await context.BreedingRecords.FindAsync(breedingRecordId);
            if (breedingRecord == null) throw new Exception("Breeding Record not found");
            var livestock = await context.Livestocks.Include(b => b.Breed).FirstOrDefaultAsync(l => l.Id == breedingRecord.FemaleId);
            if (livestock == null || livestock.Gender == GenderConstants.Male) throw new Exception("Female Livestock not found");

            entitiesModified.Add(new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Modified", breedingRecord.ModifiedBy));
            entitiesModified.Add(new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy));

            var birthDuty = await context.Duties.FirstOrDefaultAsync(d => d.LivestockAnimal != null && d.LivestockAnimal.Id == livestock.Breed.LivestockAnimalId && d.Command == DutyCommands.Birth);
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

            if (save) await context.SaveChangesAsync(cancellationToken);
            entitiesModified.Add(new ModifiedEntity(scheduledDuty.Id.ToString(), scheduledDuty.GetType().Name, "Created", scheduledDuty.ModifiedBy));
            return entitiesModified;
        }
        private static LivestockModel CreateBirthModel(int count, Domain.Entity.BreedingRecord breedingRecord, Domain.Entity.Livestock female, Domain.Entity.LivestockStatus status, string gender)
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
                    Variety = female.Variety
                };

        public async static Task<List<CreateLivestock>> OnBreedingRecordResolved(IMicroAgManagementDbContext context, long breedingRecordId, CancellationToken cancellationToken, bool save = true)
        {
            var entitiesModified = new List<CreateLivestock>();
            var breedingRecord = await context.BreedingRecords.FindAsync(breedingRecordId);
            if (breedingRecord == null) throw new Exception("Breeding Record not found");
            if (breedingRecord.Resolution != BreedingResolutionConstants.Success || !breedingRecord.ResolutionDate.HasValue) return entitiesModified;
            var female = await context.Livestocks.Include(b => b.Breed).FirstOrDefaultAsync(l => l.Id == breedingRecord.FemaleId);
            var status = await context.LivestockStatuses.FirstOrDefaultAsync(l => l.LivestockAnimalId == female.Breed.LivestockAnimalId && l.DefaultStatus);
            for (int i = 0; i < breedingRecord.BornFemales; i++)
            {
                entitiesModified.Add(new CreateLivestock
                {
                    CreatedBy = breedingRecord.ModifiedBy,
                    TenantId = breedingRecord.TenantId,
                    CreationMode = "Birth",
                    Livestock = CreateBirthModel(i, breedingRecord, female, status, GenderConstants.Female)
                });
            }
            for (int i = 0; i < breedingRecord.BornMales; i++)
            {
                entitiesModified.Add(new CreateLivestock
                {
                    CreatedBy = breedingRecord.ModifiedBy,
                    TenantId = breedingRecord.TenantId,
                    CreationMode = "Birth",
                    Livestock = CreateBirthModel(i, breedingRecord, female, status, GenderConstants.Male)
                });
            }
            return entitiesModified;
        }
        public async static Task<List<ModifiedEntity>> OnLivestockBorn(IMicroAgManagementDbContext context, long breedingRecordId, CancellationToken cancellationToken, bool save = true)
        {
            var entitiesModified = new List<ModifiedEntity>();
            return entitiesModified;
        }
    }
}


















