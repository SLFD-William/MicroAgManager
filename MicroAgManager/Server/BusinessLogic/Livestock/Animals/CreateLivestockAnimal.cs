using BackEnd.Abstracts;
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
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockAnimal request, CancellationToken cancellationToken)
            {
                var LivestockAnimal = new Domain.Entity.LivestockAnimal(request.ModifiedBy, request.TenantId);
                LivestockAnimal = request.LivestockAnimal.MapToEntity(LivestockAnimal);
                LivestockAnimal.ModifiedOn = LivestockAnimal.CreatedOn = DateTime.Now;
                LivestockAnimal.ModifiedBy = LivestockAnimal.CreatedBy = request.ModifiedBy;
                LivestockAnimal.TenantId = request.TenantId;
                _context.LivestockAnimals.Add(LivestockAnimal);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new LivestockAnimalCreated { EntityName=LivestockAnimal.GetType().Name, Id= LivestockAnimal.Id, ModifiedBy= LivestockAnimal.ModifiedBy,TenantId= LivestockAnimal.TenantId }, cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return LivestockAnimal.Id;
            }
        }
    }
}
