using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Event
{
    public class GetEvent : EventQueries, IRequest<EventModel?>
    {
        public long Id { get; set; }
        public class Handler : IRequestHandler<GetEvent, EventModel?>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<EventModel?> Handle(GetEvent request, CancellationToken cancellationToken) =>
                EventModel.Create(await request.GetQuery<Domain.Entity.Event>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
