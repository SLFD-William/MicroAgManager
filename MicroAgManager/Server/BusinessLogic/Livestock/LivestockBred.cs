using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class LivestockBred : BaseNotification
    {
        public class LivestockBredHandler : BaseNotificationHandler<LivestockBred>
        {
            public LivestockBredHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task Handle(LivestockBred notification, CancellationToken cancellationToken)
            {
                var modifiedNotice = await LivestockLogic.OnLivestockBred(_context, notification.Id, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(notification.TenantId, modifiedNotice), cancellationToken);
            }
        }
    }
}
