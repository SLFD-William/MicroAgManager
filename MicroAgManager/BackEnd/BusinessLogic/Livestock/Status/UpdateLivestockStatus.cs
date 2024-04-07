using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class UpdateLivestockStatus:BaseCommand,IUpdateCommand
    {
        public Domain.Models.LivestockStatusModel LivestockStatus { get; set; }

        public class Handler : BaseCommandHandler<UpdateLivestockStatus>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockStatus request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var livestockStatus = request.LivestockStatus.Map(await context.LivestockStatuses.FirstAsync(l=>l.Id==request.LivestockStatus.Id && request.TenantId==l.TenantId)) as Domain.Entity.LivestockStatus;
                    livestockStatus.ModifiedBy = request.ModifiedBy;
                    livestockStatus.TenantId = request.TenantId;

                    context.LivestockStatuses.Update(livestockStatus);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, LivestockStatusModel.Create(livestockStatus).GetJsonString(), nameof(LivestockStatusModel), livestockStatus.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                    return livestockStatus.Id;
                }
            }
        }

    }
}
