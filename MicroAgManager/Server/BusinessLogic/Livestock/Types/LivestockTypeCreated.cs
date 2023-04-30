using BackEnd.Abstracts;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class LivestockTypeCreated : BaseNotification
    {
        public class LivestockTypeCreatedHandler : BaseNotificationHandler
        {
            public LivestockTypeCreatedHandler(IMediator mediator, IMicroAgManagementDbContext context) : base(mediator, context)
            {
            }

            public override Task Handle(BaseNotification notification, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
