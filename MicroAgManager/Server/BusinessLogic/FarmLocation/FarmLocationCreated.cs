using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class FarmLocationCreated:BaseNotification
    {
        public class FarmLocationCreatedHandler : BaseNotificationHandler<FarmLocationCreated>
        {
            public FarmLocationCreatedHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task Handle(FarmLocationCreated notification, CancellationToken cancellationToken)
            {
                var modifiedNotice = await FarmLocationLogic.OnFarmLocationCreated(_context, notification.Id, cancellationToken);
                await _mediator.Publish(new EntitiesModifiedNotification(notification.TenantId, modifiedNotice), cancellationToken);
            }
        }
    }
}
