using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.Abstracts
{
    public abstract class BaseCommandHandler<T> :Base, IRequestHandler<T, long> where T : BaseCommand
    {
        protected BaseCommandHandler(IMediator mediator, ILogger log) : base(mediator, log)
        {
        }

        public abstract Task<long> Handle(T request, CancellationToken cancellationToken);
    }
}
