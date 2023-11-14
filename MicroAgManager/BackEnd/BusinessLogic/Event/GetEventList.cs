using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Event
{
    public class GetEventList:EventQueries, IRequest<EventDto>
    {
        public class Handler : BaseRequestHandler<GetEventList>, IRequestHandler<GetEventList, EventDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<EventDto> Handle(GetEventList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Event>(context);
                    return new EventDto(await query.LongCountAsync(cancellationToken), await query.Select(f => EventModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
