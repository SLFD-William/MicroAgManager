using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class UpdateLivestockBreed:BaseCommand, IUpdateCommand
    {
        public LivestockBreedModel LivestockBreed { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestockBreed>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateLivestockBreed request, CancellationToken cancellationToken)
            {
                var livestockBreed = _context.LivestockBreeds.First(f => f.TenantId == request.TenantId && f.Id == request.LivestockBreed.Id);
                livestockBreed = request.LivestockBreed.MapToEntity(livestockBreed);
                livestockBreed.ModifiedOn = DateTime.Now;
                livestockBreed.ModifiedBy = request.ModifiedBy;
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockBreed.Id.ToString(), livestockBreed.GetType().Name, "Modified", livestockBreed.ModifiedBy) }), cancellationToken);
                return livestockBreed.Id;
            }
        }
    }
}
