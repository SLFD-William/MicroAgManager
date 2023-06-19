using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class GetLivestockTypeList : LivestockTypeQueries, IRequest<Tuple<long, ICollection<LivestockTypeModel?>>>
    {
        public class Handler : BaseRequestHandler<GetLivestockTypeList>, IRequestHandler<GetLivestockTypeList, Tuple<long, ICollection<LivestockTypeModel?>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<LivestockTypeModel?>>> Handle(GetLivestockTypeList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.LivestockType>(_context);

                return new Tuple<long, ICollection<LivestockTypeModel?>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => LivestockTypeModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}
