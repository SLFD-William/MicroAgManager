using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Unit
{
    public class GetUnitList:UnitQueries, IRequest<UnitDto>
    {
        public class Handler : BaseRequestHandler<GetUnitList>, IRequestHandler<GetUnitList, UnitDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public async Task<UnitDto> Handle(GetUnitList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery<Domain.Entity.Unit>(context);
                    return new UnitDto(await query.LongCountAsync(cancellationToken), await query.Select(f => UnitModel.Create(f)).ToListAsync(cancellationToken));
                }
            }
        }

    }
}
