using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.Abstracts
{
    public abstract class BaseCommandHandler<T> :Base, IRequestHandler<T, long> where T : BaseCommand
    {
        protected BaseCommandHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
        {
        }

        public abstract Task<long> Handle(T request, CancellationToken cancellationToken);
    }
}
