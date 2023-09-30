using BackEnd.Abstracts;
using BackEnd.BusinessLogic.Livestock.Animals;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Logic;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class ServiceLivestock : BaseCommand
    {
        public long StudId { get; set; }
        public required DateTime ServiceDate { get; set; }
        public List<long> DamIds { get; set; } = new();
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
                await LivestockLogic.VerifyNoOpenBreedingRecord(dams.Select(d=>d.Id).ToList(),request.TenantId,_context,cancellationToken);
                var modified = new List<ModifiedEntity>();
                foreach (var dam in dams)
                {
                    var service = new Domain.Entity.BreedingRecord(request.ModifiedBy, request.TenantId)
                    { 
                        FemaleId=dam.Id,
                        MaleId=stud?.Id,
                        ServiceDate=request.ServiceDate
                    };
                    _context.BreedingRecords.Add(service);
                    modified.Add(new ModifiedEntity(dam.Id.ToString(), dam.GetType().Name, "Modified", service.ModifiedBy));
                    modified.Add(new ModifiedEntity(service.Id.ToString(), service.GetType().Name, "Created", service.ModifiedBy));
                }

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    if (!request.GenerateScheduledDuties)
                    {
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, modified), cancellationToken);
                        return 0;
                    }
                    foreach (var dam in dams)
                        await _mediator.Publish(new LivestockBred { EntityName =dam.GetType().Name, Id = dam.Id, ModifiedBy = request.ModifiedBy, TenantId =request.TenantId }, cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Service Livestock"); }
                return 0;
            }
        }
    }
}
