using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class UpdateBreedingRecord : BaseCommand, IUpdateCommand
    {
        public BreedingRecordModel BreedingRecord { get; set; }
public class Handler: BaseCommandHandler<UpdateBreedingRecord>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateBreedingRecord request, CancellationToken cancellationToken)
            {
                var breedingRecord = _context.BreedingRecords.First(d => d.TenantId == request.TenantId && d.Id == request.BreedingRecord.Id);
                var resolutionNewleyChanged = breedingRecord.Resolution != request.BreedingRecord.Resolution && !breedingRecord.ResolutionDate.HasValue;
                
                breedingRecord = request.BreedingRecord.MapToEntity(breedingRecord);
                breedingRecord.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                if (resolutionNewleyChanged)
                    await _mediator.Publish(new BreedingRecordResolved { EntityName = breedingRecord.GetType().Name, Id = breedingRecord.Id, ModifiedBy = breedingRecord.ModifiedBy, TenantId = breedingRecord.TenantId }, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Modified", breedingRecord.ModifiedBy) }), cancellationToken);
                return breedingRecord.Id;

            }
        }
    }
}
