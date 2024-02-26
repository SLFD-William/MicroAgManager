using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class UpdateLivestockBreed:BaseCommand, IUpdateCommand
    {
        public LivestockBreedModel LivestockBreed { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestockBreed>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockBreed request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var livestockBreed = request.LivestockBreed.Map(await context.LivestockBreeds.FirstAsync(f => f.TenantId == request.TenantId && f.Id == request.LivestockBreed.Id)) as LivestockBreed;
                    livestockBreed.ModifiedBy = request.ModifiedBy;
                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockBreed.Id.ToString(), livestockBreed.GetType().Name, "Modified", livestockBreed.ModifiedBy, livestockBreed.ModifiedOn) }), cancellationToken);
                    return livestockBreed.Id;
                }
            }
        }
    }
}
