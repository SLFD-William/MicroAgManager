using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Event
{
    public class GetEventList:EventQueries, IRequest<Tuple<long, ICollection<EventModel?>>>
    {
        public class Handler : BaseRequestHandler<GetEventList>, IRequestHandler<GetEventList, Tuple<long, ICollection<EventModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
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
