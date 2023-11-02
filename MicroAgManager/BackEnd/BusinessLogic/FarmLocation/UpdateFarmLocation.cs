﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class UpdateFarmLocation:BaseCommand, IUpdateCommand
    {
        public FarmLocationModel Farm { get; set; }

        public class Handler : BaseCommandHandler<UpdateFarmLocation>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateFarmLocation request, CancellationToken cancellationToken)
            {
                var farm = _context.Farms.First(f=> f.TenantId==request.TenantId && f.Id==request.Farm.Id);
                farm = request.Farm.Map(farm) as Domain.Entity.FarmLocation;
                farm.ModifiedBy = request.ModifiedBy;

                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(farm.Id.ToString(), farm.GetType().Name, "Modified", farm.ModifiedBy) }), cancellationToken);
                return farm.Id;
            }
        }
    }
}