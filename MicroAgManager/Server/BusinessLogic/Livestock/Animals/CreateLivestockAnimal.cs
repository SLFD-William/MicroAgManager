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
                var livestockAnimal = new Domain.Entity.LivestockAnimal(request.ModifiedBy, request.TenantId);
                livestockAnimal = request.LivestockAnimal.MapToEntity(livestockAnimal);
                livestockAnimal.ModifiedOn = livestockAnimal.CreatedOn = DateTime.Now;
                livestockAnimal.ModifiedBy = livestockAnimal.CreatedBy = request.ModifiedBy;
                livestockAnimal.TenantId = request.TenantId;
                _context.LivestockAnimals.Add(livestockAnimal);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new LivestockAnimalCreated { EntityName=livestockAnimal.GetType().Name, Id= livestockAnimal.Id, ModifiedBy= livestockAnimal.ModifiedBy,TenantId= livestockAnimal.TenantId }, cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockAnimal.Id;
            }
        }
    }
}
