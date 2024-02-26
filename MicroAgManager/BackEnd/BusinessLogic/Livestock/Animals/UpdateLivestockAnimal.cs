using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class UpdateLivestockAnimal : BaseCommand, IUpdateCommand
    {
        public Domain.Models.LivestockAnimalModel LivestockAnimal { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestockAnimal>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockAnimal request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var livestockAnimal = request.LivestockAnimal.Map(await context.LivestockAnimals.FirstAsync(l=>l.Id==request.LivestockAnimal.Id && request.TenantId==l.TenantId)) as LivestockAnimal;
                    livestockAnimal.ModifiedBy = request.ModifiedBy;
                    livestockAnimal.TenantId = request.TenantId;
                    context.LivestockAnimals.Update(livestockAnimal);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockAnimal.Id.ToString(), livestockAnimal.GetType().Name, "Modified", livestockAnimal.ModifiedBy, livestockAnimal.ModifiedOn) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock Type"); }
                    return livestockAnimal.Id;
                }
            }
        }
    }
}
