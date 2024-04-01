using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateLivestockStatus request, CancellationToken cancellationToken)
            {
                var livestockStatus = request.LivestockStatus.Map(new Domain.Entity.LivestockStatus(request.ModifiedBy, request.TenantId)) as Domain.Entity.LivestockStatus;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.LivestockStatuses.Add(livestockStatus);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, LivestockStatusModel.Create(livestockStatus).GetJsonString(), nameof(LivestockStatusModel)), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Livestock Status"); }
                }
                return livestockStatus.Id;
            }
        }
    }
}
