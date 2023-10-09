using BackEnd.Abstracts;
using BackEnd.BusinessLogic.Livestock.Animals;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class CreateFarmLocation:BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required]public FarmLocationModel Farm { get; set; }
        public class Handler : BaseCommandHandler<CreateFarmLocation>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateFarmLocation request, CancellationToken cancellationToken)
            {

                var farm = request.Farm.MapToEntity(new Domain.Entity.FarmLocation(request.ModifiedBy, request.TenantId));
                _context.Farms.Add(farm);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new FarmLocationCreated { EntityName = farm.GetType().Name, Id = farm.Id, ModifiedBy = farm.ModifiedBy, TenantId = farm.TenantId }, cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Farm Location"); }
                return farm.Id;
            }
        }

    }
}
