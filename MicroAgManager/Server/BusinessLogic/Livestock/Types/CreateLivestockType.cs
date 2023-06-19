using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class CreateLivestockType : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockTypeModel LivestockType { get; set; }
        public class Handler : BaseCommandHandler<CreateLivestockType>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockType request, CancellationToken cancellationToken)
            {
                var livestockType = new Domain.Entity.LivestockType(request.ModifiedBy, request.TenantId);
                livestockType = request.LivestockType.MapToEntity(livestockType);
                livestockType.ModifiedOn = livestockType.CreatedOn = DateTime.Now;
                livestockType.ModifiedBy = livestockType.CreatedBy = request.ModifiedBy;
                livestockType.TenantId = request.TenantId;
                _context.LivestockTypes.Add(livestockType);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new LivestockTypeCreated { EntityName=livestockType.GetType().Name, Id= livestockType.Id, ModifiedBy= livestockType.ModifiedBy,TenantId= livestockType.TenantId }, cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockType.Id;
            }
        }
    }
}
