using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Duty
{
    public class GetDutyList:DutyQueries, IRequest<Tuple<long, ICollection<DutyModel?>>>
    {
        public class Handler : BaseRequestHandler<GetDutyList>, IRequestHandler<GetDutyList, Tuple<long, ICollection<DutyModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
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
