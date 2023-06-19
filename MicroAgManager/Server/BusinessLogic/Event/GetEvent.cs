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
        public long Id { get; set; }
        public class Handler : BaseRequestHandler<GetEvent>, IRequestHandler<GetEvent, EventModel?>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<EventModel?> Handle(GetEvent request, CancellationToken cancellationToken) =>
                EventModel.Create(await request.GetQuery<Domain.Entity.Event>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
