using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Logic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Animals
{
    public class LivestockAnimalCreated : BaseNotification
    {
        public class LivestockAnimalCreatedHandler : BaseNotificationHandler<LivestockAnimalCreated>
        {
            public LivestockAnimalCreatedHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task Handle(LivestockAnimalCreated notification, CancellationToken cancellationToken)
            {
                var modifiedNotice = await LivestockAnimalLogic.OnLivestockAnimalCreated(_context, notification.Id, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(notification.TenantId, modifiedNotice), cancellationToken);
            }
        }
    }
}
