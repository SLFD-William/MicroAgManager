﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class UpdateBreedingRecord : BaseCommand, IUpdateCommand
    {
        public BreedingRecordModel BreedingRecord { get; set; }
public class Handler: BaseCommandHandler<UpdateBreedingRecord>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateBreedingRecord request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var breedingRecord = await context.BreedingRecords.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.BreedingRecord.Id);
                    var resolutionNewleyChanged = breedingRecord.Resolution != request.BreedingRecord.Resolution && !breedingRecord.ResolutionDate.HasValue;

                    breedingRecord = request.BreedingRecord.Map(breedingRecord) as Domain.Entity.BreedingRecord;
                    breedingRecord.ModifiedBy = request.ModifiedBy;
                    await context.SaveChangesAsync(cancellationToken);
                    if (resolutionNewleyChanged)
                    {
                        var createLivestocks = await LivestockLogic.OnBreedingRecordResolved(context, breedingRecord.Id);
                        foreach (var livestock in createLivestocks)
                            await _mediator.Send(livestock, cancellationToken);
                    }
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Modified", breedingRecord.ModifiedBy) }), cancellationToken);
                    return breedingRecord.Id;
                }

            }
        }
    }
}
