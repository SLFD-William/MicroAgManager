using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.BusinessLogic.Livestock
{
    public class ServiceLivestock : BaseCommand, IHasRecipient
    {
        [Required] public long StudId { get; set; }
        public required DateTime ServiceDate { get; set; }
        [Required] public required List<long> DamIds { get; set; }
        public string Notes { get; set; }
        public bool GenerateScheduledDuties { get; set; } = true;
        public long RecipientTypeId { get; set; }
        [Required] public long ScheduleSourceId { get; set; }
        [Required] public string ScheduleSource { get; set; } //Chore,Event,Milestone

        public string RecipientType { get; set; }
        public long RecipientId { get => StudId; set => StudId=value; }
        [NotMapped] public string RecipientTypeItem { get; set; } = string.Empty;
        [NotMapped] public string RecipientItem { get; set; } = string.Empty;

        public class Handler : BaseCommandHandler<ServiceLivestock>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(ServiceLivestock request, CancellationToken cancellationToken)
            {
                if (!request.DamIds.Any())
                    return 0;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var stud = await context.Livestocks.FirstAsync(s=>s.Id==request.StudId && s.TenantId==request.TenantId);
                    
                    var dams =await  context.Livestocks.Include(b => b.Breed).ThenInclude(a=>a.LivestockAnimal).Where(f => request.DamIds.Contains(f.Id)).ToListAsync();
                    //await LivestockLogic.VerifyNoOpenBreedingRecord(dams.Select(d=>d.Id).ToList(),request.TenantId,context,cancellationToken);
                    var modified = new List<Domain.Entity.BreedingRecord>();

                    foreach (var dam in dams)
                    {
                        var service = new Domain.Entity.BreedingRecord(request.ModifiedBy, request.TenantId)
                        {
                            FemaleId = dam.Id,
                            RecipientType=dam.Breed.LivestockAnimal.GetType().Name,
                            RecipientTypeId=dam.Breed.LivestockAnimalId,
                            MaleId = stud?.Id,
                            ServiceDate = request.ServiceDate,
                            Notes = request.Notes ?? string.Empty
                        };
                        modified.Add(service);
                    }
                    context.BreedingRecords.AddRange(modified);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        if (request.GenerateScheduledDuties)
                            foreach (var dam in modified)
                            {
                                try { 
                                    var modifiedNotice = await LivestockLogic.OnLivestockBred(context, dam.Id, request.ScheduleSource, request.ScheduleSourceId, cancellationToken);
                                    foreach (var mod in modifiedNotice)
                                        await _mediator.Publish(new ModifiedEntityPushNotification(mod.TenantId, mod.ModelJson, mod.ModelType,mod.ServerModifiedTime), cancellationToken);
                                   
                                }
                                catch (Exception ex) { _log.LogError(ex, $"{ex.Message} {ex.StackTrace}"); }
                            }
                        foreach (var breedingRecord in modified)
                            await _mediator.Publish(new ModifiedEntityPushNotification(breedingRecord.TenantId, BreedingRecordModel.Create(breedingRecord).GetJsonString(), nameof(BreedingRecordModel),breedingRecord.ModifiedOn), cancellationToken);

                    }
                    catch (Exception ex) { _log.LogError(ex, $"{ex.Message} {ex.StackTrace}"); }
                }
                return 0;
            }
        }
    }
}
