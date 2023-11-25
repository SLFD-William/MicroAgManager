using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.Abstracts
{
    public abstract class BaseNotificationHandler<T> : Base, INotificationHandler<T> where T : BaseNotification
    {
        protected BaseNotificationHandler(IMediator mediator, ILogger log) : base(mediator, log)
        {
        }

        public abstract Task Handle(T notification, CancellationToken cancellationToken);
    }
}