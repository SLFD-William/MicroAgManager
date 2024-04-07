using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Feed
{
    //Create a class called UpdateLivestockFeed implementing BaseCommand and IUpdateCommand
    public class UpdateLivestockFeed : BaseCommand, IUpdateCommand
    {
        //Add a property called LivestockFeed of type LivestockFeedModel
        public Domain.Models.LivestockFeedModel LivestockFeed { get; set; }
        //create a handler class implementing BaseCommandHandler<UpdateLivestockFeed>
        public class Handler : BaseCommandHandler<UpdateLivestockFeed>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockFeed request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var livestockFeed = request.LivestockFeed.Map(await context.LivestockFeeds.FirstAsync(l=>l.Id==request.LivestockFeed.Id && request.TenantId==l.TenantId)) as Domain.Entity.LivestockFeed;
                    livestockFeed.ModifiedBy = request.ModifiedBy;
                    livestockFeed.TenantId = request.TenantId;
                    context.LivestockFeeds.Update(livestockFeed);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification (livestockFeed.TenantId, LivestockFeedModel.Create(livestockFeed).GetJsonString(), nameof(LivestockFeedModel), livestockFeed.ModifiedOn), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock Feed"); }
                    return livestockFeed.Id;
                }

            }
        }
    }
}
