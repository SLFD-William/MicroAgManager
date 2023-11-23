using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Event
{
    public class GetEvent : EventQueries, IRequest<EventModel?>
    {
        public class Handler : BaseRequestHandler<GetEvent>, IRequestHandler<GetEvent, EventModel?>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<EventModel?> Handle(GetEvent request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return EventModel.Create(await request.GetQuery<Domain.Entity.Event>(context).FirstOrDefaultAsync(cancellationToken), context);
                }
            }
        }
    }
}
