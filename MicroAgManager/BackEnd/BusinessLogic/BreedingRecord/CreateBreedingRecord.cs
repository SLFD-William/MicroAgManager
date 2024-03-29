﻿using BackEnd.Abstracts;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateBreedingRecord request, CancellationToken cancellationToken)
            {
                var breedingRecord = new Domain.Entity.BreedingRecord(request.ModifiedBy, request.TenantId) 
                { FemaleId =request.BreedingRecord.FemaleId};
                breedingRecord = request.BreedingRecord.Map(breedingRecord) as Domain.Entity.BreedingRecord;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.BreedingRecords.Add(breedingRecord);
                    var resolutionNewleyChanged = !string.IsNullOrEmpty(breedingRecord.Resolution) && breedingRecord.ResolutionDate.HasValue;
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        if (resolutionNewleyChanged)
                        {
                            var createLivestocks = await LivestockLogic.OnBreedingRecordResolved(context, breedingRecord.Id);
                            foreach (var livestock in createLivestocks)
                                await _mediator.Send(livestock, cancellationToken);
                        }
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(breedingRecord.Id.ToString(), breedingRecord.GetType().Name, "Created", breedingRecord.ModifiedBy) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create BreedingRecord"); }
                }
                return breedingRecord.Id;
            }
        }
    }
}
