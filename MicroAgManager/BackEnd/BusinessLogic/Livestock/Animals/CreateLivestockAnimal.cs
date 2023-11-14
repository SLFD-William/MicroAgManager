using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class CreateLivestockAnimal : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockAnimalModel LivestockAnimal { get; set; }
        public class Handler : BaseCommandHandler<CreateLivestockAnimal>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockAnimal request, CancellationToken cancellationToken)
            {
                var livestockAnimal = request.LivestockAnimal.Map(new LivestockAnimal(request.ModifiedBy, request.TenantId)) as LivestockAnimal;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.LivestockAnimals.Add(livestockAnimal);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        var modifiedNotice = await LivestockAnimalLogic.OnLivestockAnimalCreated(context, livestockAnimal.Id, cancellationToken);
                        await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, modifiedNotice), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                return livestockAnimal.Id;
            }
        }
    }
}
