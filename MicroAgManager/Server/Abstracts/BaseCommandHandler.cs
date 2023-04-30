using Domain.Interfaces;
using MediatR;

namespace BackEnd.Abstracts
{
    public abstract class BaseCommandHandler<T> : IRequestHandler<T, long> where T : BaseCommand
    {
        protected readonly IMicroAgManagementDbContext _context;
        protected readonly IMediator _mediator;

        protected BaseCommandHandler(IMicroAgManagementDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public abstract Task<long> Handle(T request, CancellationToken cancellationToken);
    }
}
