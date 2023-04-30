using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.BusinessLogic.Duty
{
    public class GetDutyList:DutyQueries, IRequest<Tuple<long, ICollection<DutyModel?>>>
    {
        public class Handler : IRequestHandler<GetDutyList, Tuple<long, ICollection<DutyModel?>>>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<Tuple<long, ICollection<DutyModel?>>> Handle(GetDutyList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Duty>(_context);
                return new Tuple<long, ICollection<DutyModel?>>
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => DutyModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
