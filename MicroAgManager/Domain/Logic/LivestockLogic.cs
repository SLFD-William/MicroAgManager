using Domain.Constants;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
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
            var livestock = await context.Livestocks.Include(b=>b.Breed).FirstOrDefaultAsync(l=>l.Id==breedingRecord.FemaleId);
            if (livestock == null || livestock.Gender==GenderConstants.Male) throw new Exception("Female Livestock not found");
            
            entitiesModified.Add(new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Modified", breedingRecord.ModifiedBy));
            entitiesModified.Add(new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy));

            var birthDuty= await context.Duties.FirstOrDefaultAsync(d => d.LivestockAnimal !=null && d.LivestockAnimal.Id == livestock.Breed.LivestockAnimalId && d.Command == DutyCommands.Birth);
            if (birthDuty == null) throw new Exception("Birth Duty not found");

            var scheduledDuty = new ScheduledDuty(breedingRecord.ModifiedBy, breedingRecord.TenantId)
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
    }
}
