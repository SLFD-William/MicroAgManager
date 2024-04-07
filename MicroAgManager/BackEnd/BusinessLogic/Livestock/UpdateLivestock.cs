using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class UpdateLivestock : BaseCommand, IUpdateCommand
    {
        public Domain.Models.LivestockModel Livestock { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestock>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestock request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var livestock = request.Livestock.Map(await context.Livestocks.FirstAsync(l=>l.Id==request.Livestock.Id && request.TenantId==l.TenantId)) as Domain.Entity.Livestock;
                    livestock.ModifiedBy = request.ModifiedBy;
                    livestock.TenantId = request.TenantId;
                    context.Livestocks.Update(livestock);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        
                        await _mediator.Publish(new ModifiedEntityPushNotification(request.TenantId, LivestockModel.Create(livestock).GetJsonString(),nameof(LivestockModel), livestock.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock"); }
                    return livestock.Id;
                }
            }
        }
    }
}
