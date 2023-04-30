using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Event
{
    public class GetEventList:EventQueries, IRequest<Tuple<long, ICollection<EventModel?>>>
    {
        public class Handler : IRequestHandler<GetEventList, Tuple<long, ICollection<EventModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<EventModel?>>> Handle(GetEventList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Event>(_context);
                return new Tuple<long, ICollection<EventModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => EventModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
