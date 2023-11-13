using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Duty
{
    public class GetDutyList:DutyQueries, IRequest<DutyDto>
    {
        public class Handler : BaseRequestHandler<GetDutyList>, IRequestHandler<GetDutyList, DutyDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<DutyDto> Handle(GetDutyList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Duty>(_context);
                return new DutyDto
                    (await query.LongCountAsync(cancellationToken),
                    await query.Select(f => DutyModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
