using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class CreateLivestockStatus:BaseCommand,ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LivestockStatusModel LivestockStatus { get; set; }
        public class Handler : BaseCommandHandler<CreateLivestockStatus>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockStatus request, CancellationToken cancellationToken)
            {
                var livestockStatus = request.LivestockStatus.Map(new Domain.Entity.LivestockStatus(request.ModifiedBy, request.TenantId)) as Domain.Entity.LivestockStatus;
                _context.LivestockStatuses.Add(livestockStatus);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockStatus.Id.ToString(), livestockStatus.GetType().Name, "Created", livestockStatus.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Create Livestock Status"); }
                return livestockStatus.Id;
            }
        }
    }
}
