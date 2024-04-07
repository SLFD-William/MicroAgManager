using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Models;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateFarmLocation request, CancellationToken cancellationToken)
            {

                var farm = request.Farm.Map(new Domain.Entity.FarmLocation(request.ModifiedBy, request.TenantId)) as Domain.Entity.FarmLocation;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Farms.Add(farm);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        var modifiedNotice = await FarmLocationLogic.OnFarmLocationCreated(context, farm.Id, cancellationToken);
                        foreach(var mod in modifiedNotice)
                            await _mediator.Publish(new ModifiedEntityPushNotification (mod.TenantId, mod.ModelJson, mod.ModelType,mod.ServerModifiedTime), cancellationToken);
                        
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Farm Location"); }
                }
                return farm.Id;
            }
        }
    }
}
