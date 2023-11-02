using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.Abstracts
{
    public abstract class BaseNotificationHandler<T> : Base, INotificationHandler<T> where T : BaseNotification
    {
        protected BaseNotificationHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
        {
        }

        public abstract Task Handle(T notification, CancellationToken cancellationToken);
    }
}