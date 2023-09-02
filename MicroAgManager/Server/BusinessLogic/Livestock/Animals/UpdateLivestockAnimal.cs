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
                var LivestockAnimal = _context.LivestockAnimals.Find(request.LivestockAnimal.Id);
                LivestockAnimal = request.LivestockAnimal.MapToEntity(LivestockAnimal);
                LivestockAnimal.ModifiedOn = DateTime.Now;
                LivestockAnimal.ModifiedBy = request.ModifiedBy;
                LivestockAnimal.TenantId = request.TenantId;
                _context.LivestockAnimals.Update(LivestockAnimal);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(LivestockAnimal.Id.ToString(), LivestockAnimal.GetType().Name, "Modified", LivestockAnimal.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock Type"); }
                return LivestockAnimal.Id;
            }
        }
    }
}
