using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class LivestockBorn:BaseNotification
    {
        public class LivestockBornHandler : BaseNotificationHandler<LivestockBorn>
        {
            public LivestockBornHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task Handle(LivestockBorn notification, CancellationToken cancellationToken)
            {
                var modifiedNotice = await LivestockLogic.OnLivestockBorn(_context, notification.Id, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(notification.TenantId, modifiedNotice), cancellationToken);
            }
        }
    }
}
