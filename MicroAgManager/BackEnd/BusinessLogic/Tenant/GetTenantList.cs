using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Tenant
{
    public class GetTenantList:TenantQueries, IRequest<TenantDto>
    {
        public class Handler : BaseRequestHandler<GetTenantList>, IRequestHandler<GetTenantList, TenantDto>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }
            public async Task<TenantDto> Handle(GetTenantList request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var query = request.GetQuery(context);
                    var results = new TenantDto(await query.LongCountAsync(cancellationToken), await query.Select(f => TenantModel.Create(f)).ToListAsync(cancellationToken));
                    return results;
                }
            }
        }
    }
}
