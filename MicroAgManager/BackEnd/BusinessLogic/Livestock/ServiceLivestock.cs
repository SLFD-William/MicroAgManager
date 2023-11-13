using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(ServiceLivestock request, CancellationToken cancellationToken)
            {
                if (!request.DamIds.Any())
                    return 0;
                var stud = _context.Livestocks.Find(request.StudId);
                var dams = _context.Livestocks.Where(f => request.DamIds.Contains(f.Id)).ToList();
                //await LivestockLogic.VerifyNoOpenBreedingRecord(dams.Select(d=>d.Id).ToList(),request.TenantId,_context,cancellationToken);
                var modified = new List<Domain.Entity.BreedingRecord>();

                foreach (var dam in dams)
                {
                    var service = new Domain.Entity.BreedingRecord(request.ModifiedBy, request.TenantId)
                    { 
                        FemaleId=dam.Id,
                        MaleId=stud?.Id,
                        ServiceDate=request.ServiceDate,
                        Notes=request.Notes ?? string.Empty
                    };
                    modified.Add(service);
                }
                _context.BreedingRecords.AddRange(modified);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    if (request.GenerateScheduledDuties)
                        foreach (var dam in modified)
                            await _mediator.Publish(new LivestockBred { EntityName = dam.GetType().Name, Id = dam.Id, ModifiedBy = request.ModifiedBy, TenantId = request.TenantId }, cancellationToken);

                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, modified.Select(b=> new ModifiedEntity(b.Id.ToString(), b.GetType().Name,"Created",b.ModifiedBy)).ToList()), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, $"{ex.Message} {ex.StackTrace}"); }
                return 0;
            }
        }
    }
}
