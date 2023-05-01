using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.Logic;
using MediatR;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class LivestockTypeCreated : BaseNotification
    {
        public class LivestockTypeCreatedHandler : INotificationHandler<LivestockTypeCreated>
        {
            protected readonly IMediator _mediator;
            protected readonly IMicroAgManagementDbContext _context;

            public LivestockTypeCreatedHandler(IMediator mediator, IMicroAgManagementDbContext context)
            {
                _mediator = mediator;
                _context = context;
            }

            public async Task Handle(LivestockTypeCreated notification, CancellationToken cancellationToken)
            {
                var modifiedNotice = await LivestockTypeLogic.OnLivestockTypeCreated(_context, notification.Id, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(notification.TenantId, modifiedNotice), cancellationToken);
            }
        }
    }
}
