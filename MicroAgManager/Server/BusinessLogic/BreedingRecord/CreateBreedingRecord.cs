using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class CreateBreedingRecord : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public BreedingRecordModel BreedingRecord { get; set; }
        public class Handler : BaseCommandHandler<CreateBreedingRecord>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateBreedingRecord request, CancellationToken cancellationToken)
            {
                var breedingRecord = new Domain.Entity.BreedingRecord(request.ModifiedBy, request.TenantId) 
                { FemaleId =request.BreedingRecord.FemaleId};
                breedingRecord = request.BreedingRecord.MapToEntity(breedingRecord);
                _context.BreedingRecords.Add(breedingRecord);
                var resolutionNewleyChanged = !string.IsNullOrEmpty(breedingRecord.Resolution) && breedingRecord.ResolutionDate.HasValue;
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    if (resolutionNewleyChanged)
                        await _mediator.Publish(new BreedingRecordResolved { EntityName = breedingRecord.GetType().Name, Id = breedingRecord.Id, ModifiedBy = breedingRecord.ModifiedBy, TenantId = breedingRecord.TenantId }, cancellationToken);

                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create BreedingRecord"); }
                return breedingRecord.Id;
            }
        }
    }
}
