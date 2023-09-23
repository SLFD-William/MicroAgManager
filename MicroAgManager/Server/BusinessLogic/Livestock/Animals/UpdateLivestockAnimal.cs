using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class UpdateLivestockAnimal : BaseCommand, IUpdateCommand
    {
        public Domain.Models.LivestockAnimalModel LivestockAnimal { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestockAnimal>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockAnimal request, CancellationToken cancellationToken)
            {
                var livestockAnimal = _context.LivestockAnimals.Find(request.LivestockAnimal.Id);
                livestockAnimal = request.LivestockAnimal.MapToEntity(livestockAnimal);
                livestockAnimal.ModifiedBy = request.ModifiedBy;
                livestockAnimal.TenantId = request.TenantId;
                _context.LivestockAnimals.Update(livestockAnimal);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockAnimal.Id.ToString(), livestockAnimal.GetType().Name, "Modified", livestockAnimal.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock Type"); }
                return livestockAnimal.Id;
            }
        }
    }
}
