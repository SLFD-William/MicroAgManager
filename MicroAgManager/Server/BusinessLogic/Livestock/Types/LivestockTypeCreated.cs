using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class LivestockTypeCreated : BaseNotification
    {
        public class LivestockTypeCreatedHandler : BaseNotificationHandler<LivestockTypeCreated>
        {
            public LivestockTypeCreatedHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task Handle(LivestockTypeCreated notification, CancellationToken cancellationToken)
            {
                var modifiedNotice = await LivestockTypeLogic.OnLivestockTypeCreated(_context, notification.Id, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(notification.TenantId, modifiedNotice), cancellationToken);
            }
        }
    }
}
