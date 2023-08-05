using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Duty
{
    public class GetDuty : DutyQueries, IRequest<DutyModel?>
    {
        public class Handler : BaseRequestHandler<GetDuty>, IRequestHandler<GetDuty, DutyModel?>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<DutyModel?> Handle(GetDuty request, CancellationToken cancellationToken) =>
                DutyModel.Create(await request.GetQuery<Domain.Entity.Duty>(_context).FirstOrDefaultAsync(cancellationToken));
        }
    }
}
