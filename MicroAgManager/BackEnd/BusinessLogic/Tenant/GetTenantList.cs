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
            private readonly ILogger _log;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
                _log = log;
                //_log.Log(LogLevel.Information, "Getting Tenant List");
            }

            public async Task<TenantDto> Handle(GetTenantList request, CancellationToken cancellationToken)
            {
                
                var query = request.GetQuery(_context);
                var results = new TenantDto
                (
                     await query.LongCountAsync(cancellationToken),
                     await query.Select(f => TenantModel.Create(f)).ToListAsync(cancellationToken)
                );
                //_log.Log(LogLevel.Information, $"Returning {results.Item1} Tennant");
                return results;
            }
        }
    }
}
