using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock
{
    public class ServiceLivestock : BaseCommand
    {
        [Required]public long StudId { get; set; }
        public required DateTime ServiceDate { get; set; }
        [Required] public required List<long> DamIds { get; set; }
        public string Notes { get; set; }
        public bool GenerateScheduledDuties { get; set; } = true;
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
                    var dams =await  context.Livestocks.Where(f => request.DamIds.Contains(f.Id)).ToListAsync();
                    //await LivestockLogic.VerifyNoOpenBreedingRecord(dams.Select(d=>d.Id).ToList(),request.TenantId,context,cancellationToken);
                    var modified = new List<Domain.Entity.BreedingRecord>();

                    foreach (var dam in dams)
                    {
                        var service = new Domain.Entity.BreedingRecord(request.ModifiedBy, request.TenantId)
                        {
                            FemaleId = dam.Id,
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
                                    var modifiedNotice = await LivestockLogic.OnLivestockBred(context, dam.Id, cancellationToken);
                                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, modifiedNotice), cancellationToken);
                                }
                                catch (Exception ex) { _log.LogError(ex, $"{ex.Message} {ex.StackTrace}"); }
                            }
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, modified.Select(b => new ModifiedEntity(b.Id.ToString(), b.GetType().Name, "Created", b.ModifiedBy)).ToList()), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, $"{ex.Message} {ex.StackTrace}"); }
                }
                return 0;
            }
        }
    }
}
