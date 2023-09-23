using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.LivestockFeed
{
    //Create a class called UpdateLivestockFeed implementing BaseCommand and IUpdateCommand
    public class UpdateLivestockFeed : BaseCommand, IUpdateCommand
    {
        //Add a property called LivestockFeed of type LivestockFeedModel
        public Domain.Models.LivestockFeedModel LivestockFeed { get; set; }
        //create a handler class implementing BaseCommandHandler<UpdateLivestockFeed>
        public class Handler : BaseCommandHandler<UpdateLivestockFeed>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockFeed request, CancellationToken cancellationToken)
            {
                var livestockFeed = _context.LivestockFeeds.Find(request.LivestockFeed.Id);
                livestockFeed = request.LivestockFeed.MapToEntity(livestockFeed);
                livestockFeed.ModifiedBy = request.ModifiedBy;
                livestockFeed.TenantId = request.TenantId;
                _context.LivestockFeeds.Update(livestockFeed);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockFeed.Id.ToString(), livestockFeed.GetType().Name, "Modified", livestockFeed.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock Feed"); }
                return livestockFeed.Id;

            }
        }
    }
}
