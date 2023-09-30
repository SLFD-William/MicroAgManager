using Domain.Abstracts;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async static Task<List<ModifiedEntity>> OnLivestockBred(IMicroAgManagementDbContext context, long id, CancellationToken cancellationToken, bool save = true)
        {
            var entitiesModified = new List<ModifiedEntity>();
            var livestock = await context.Livestocks.FindAsync(id);
            if (livestock == null || livestock.Gender==GenderConstants.Male) throw new Exception("Female Livestock not found");
            var breedingRecord = await context.BreedingRecords.FirstOrDefaultAsync(b => b.FemaleId == id && !b.ResolutionDate.HasValue);
            if (breedingRecord == null) throw new Exception("Breeding Record not found");
            entitiesModified.Add(new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Modified", breedingRecord.ModifiedBy));
            entitiesModified.Add(new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy));
            //TODO Schedule the stuff

            entitiesModified.AddRange(await EntityLogic.GetModifiedEntities(context));

            if (save) await context.SaveChangesAsync(cancellationToken);
            return entitiesModified;
        }
    }
}
