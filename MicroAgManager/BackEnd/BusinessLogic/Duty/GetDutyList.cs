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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<DutyDto> Handle(GetDutyList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Duty>(context);
                    return new DutyDto(await query.LongCountAsync(cancellationToken), await query.Select(f => DutyModel.Create(f, context)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
