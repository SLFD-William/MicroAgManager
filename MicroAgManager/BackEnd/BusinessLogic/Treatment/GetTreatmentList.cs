using BackEnd.Abstracts;
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
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<TreatmentDto> Handle(GetTreatmentList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Treatment>(context);
                    return new TreatmentDto(await query.LongCountAsync(cancellationToken), await query.Select(f => TreatmentModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }
    }
}
