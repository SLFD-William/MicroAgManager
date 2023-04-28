﻿using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class UpdateFarmLocation:BaseCommand, IRequest<long>,IUpdateCommand
    {
        public FarmLocationModel Farm { get; set; }

        public class Handler : IRequestHandler<UpdateFarmLocation, long>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<long> Handle(UpdateFarmLocation request, CancellationToken cancellationToken)
            {
                var farm = _context.Farms.First(f=> f.TenantId==request.TenantId && f.Id==request.Farm.Id);
                farm = request.Farm.MapToEntity(farm);
                farm.ModifiedOn = DateTime.Now;
                farm.ModifiedBy = request.ModifiedBy;

                await _context.SaveChangesAsync(cancellationToken);
                //await _mediator.Publish(new NotificationMessage
                //{
                //    To = request.TenantId.ToString(),
                //    Body = $"{nameof(FarmLocationModel)} {farm.Id}",
                //    From = request.ModifiedBy.ToString(),
                //    Subject = "Update"
                //}, cancellationToken);
                return farm.Id;
            }
        }
    }
}
