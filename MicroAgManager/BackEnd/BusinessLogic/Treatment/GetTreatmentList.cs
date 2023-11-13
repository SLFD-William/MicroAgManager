using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Treatment
{
    public class GetTreatmentList:TreatmentQueries, IRequest<TreatmentDto>
    {
        public class Handler : BaseRequestHandler<GetTreatmentList>, IRequestHandler<GetTreatmentList, TreatmentDto>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<TreatmentDto> Handle(GetTreatmentList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Treatment>(_context);
                return new TreatmentDto
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => TreatmentModel.Create(f)).ToListAsync(cancellationToken));
            }
        }
    }
}
