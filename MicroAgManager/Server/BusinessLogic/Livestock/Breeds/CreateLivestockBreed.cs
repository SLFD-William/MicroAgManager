using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;

namespace BackEnd.BusinessLogic.Livestock.Breeds
{
    public class CreateLivestockBreed:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        public LivestockBreedModel LivestockBreed { get; set; }
        public class Handler : BaseCommandHandler<CreateLivestockBreed>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(CreateLivestockBreed request, CancellationToken cancellationToken)
            {
                var livestockBreed = new Domain.Entity.LivestockBreed(request.ModifiedBy, request.TenantId);
                livestockBreed = request.LivestockBreed.MapToEntity(livestockBreed);
                if(livestockBreed.Livestock is null)
                    livestockBreed.Livestock = await _context.LivestockTypes.FindAsync(request.LivestockBreed.LivestockTypeId);
                livestockBreed.ModifiedOn = livestockBreed.Created = DateTime.Now;
                livestockBreed.ModifiedBy = livestockBreed.CreatedBy = request.ModifiedBy;
                livestockBreed.TenantId = request.TenantId;
                _context.LivestockBreeds.Add(livestockBreed);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockBreed.Id.ToString(), livestockBreed.GetType().Name, "Created", livestockBreed.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockBreed.Id;
            }
        }
    }
}
