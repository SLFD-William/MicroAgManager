using BackEnd.Abstracts;
using Domain.Interfaces;
using MediatR;

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
