using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class CreateLivestockBreed:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        public LivestockBreedModel LivestockBreed { get; set; }
        public class Handler : BaseCommandHandler<CreateLivestockBreed>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockBreed request, CancellationToken cancellationToken)
            {
                var livestockBreed = request.LivestockBreed.Map(new Domain.Entity.LivestockBreed(request.ModifiedBy, request.TenantId)) as LivestockBreed;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    if (livestockBreed.LivestockAnimal is null)
                        livestockBreed.LivestockAnimal = await context.LivestockAnimals.FindAsync(request.LivestockBreed.LivestockAnimalId);

                    context.LivestockBreeds.Add(livestockBreed);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockBreed.Id.ToString(), livestockBreed.GetType().Name, "Created", livestockBreed.ModifiedBy) }), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Livestock Breed"); }
                }
                return livestockBreed.Id;
            }
        }
    }
}
