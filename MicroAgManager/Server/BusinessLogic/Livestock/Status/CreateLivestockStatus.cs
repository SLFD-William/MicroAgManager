using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class CreateLivestockStatus:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockStatusModel LivestockStatus { get; set; }
        public class Handler : BaseCommandHandler<CreateLivestockStatus>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }

            public override async Task<long> Handle(CreateLivestockStatus request, CancellationToken cancellationToken)
            {
                var livestockStatus = new Domain.Entity.LivestockStatus(request.ModifiedBy, request.TenantId);
                livestockStatus = request.LivestockStatus.MapToEntity(livestockStatus);
                livestockStatus.ModifiedOn = livestockStatus.CreatedOn = DateTime.Now;
                livestockStatus.ModifiedBy = livestockStatus.CreatedBy = request.ModifiedBy;
                livestockStatus.TenantId = request.TenantId;
                _context.LivestockStatuses.Add(livestockStatus);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockStatus.Id.ToString(), livestockStatus.GetType().Name, "Created", livestockStatus.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockStatus.Id;
            }
        }
    }
}
