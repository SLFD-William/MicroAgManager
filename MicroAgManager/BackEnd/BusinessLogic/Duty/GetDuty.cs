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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<DutyModel?> Handle(GetDuty request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    return DutyModel.Create(await request.GetQuery<Domain.Entity.Duty>(context).FirstOrDefaultAsync(cancellationToken));
                }
            }
        }
    }
}
