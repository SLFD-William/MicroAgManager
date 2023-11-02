﻿using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class UpdateLivestockStatus:BaseCommand,IUpdateCommand
    {
        public Domain.Models.LivestockStatusModel LivestockStatus { get; set; }

        public class Handler : BaseCommandHandler<UpdateLivestockStatus>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockStatus request, CancellationToken cancellationToken)
            {
                var livestockStatus = request.LivestockStatus.Map(_context.LivestockStatuses.Find(request.LivestockStatus.Id)) as Domain.Entity.LivestockStatus;
                livestockStatus.ModifiedBy = request.ModifiedBy;
                livestockStatus.TenantId = request.TenantId;
                _context.LivestockStatuses.Update(livestockStatus);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockStatus.Id.ToString(), livestockStatus.GetType().Name, "Modified", livestockStatus.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockStatus.Id;
            }
        }

    }
}