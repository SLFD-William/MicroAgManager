﻿using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Tenant
{
    public class GetTenantList:TenantQueries, IRequest<Tuple<long, ICollection<TenantModel>>>
    {
        public class Handler : BaseRequestHandler<GetTenantList>, IRequestHandler<GetTenantList, Tuple<long, ICollection<TenantModel>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<TenantModel>>> Handle(GetTenantList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery(_context);
                return new Tuple<long, ICollection<TenantModel>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => TenantModel.Create(f)).ToListAsync(cancellationToken)
                    );
            }
        }
    }
}