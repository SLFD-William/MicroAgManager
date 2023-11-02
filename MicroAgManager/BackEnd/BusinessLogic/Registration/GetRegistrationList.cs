using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Registration
{
    public class GetRegistrationList:RegistrationQueries, IRequest<Tuple<long, ICollection<RegistrationModel>>>
    {
        public class Handler : BaseRequestHandler<GetRegistrationList>, IRequestHandler<GetRegistrationList, Tuple<long, ICollection<RegistrationModel>>>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public async Task<Tuple<long, ICollection<RegistrationModel>>> Handle(GetRegistrationList request, CancellationToken cancellationToken)
            {
                var query = request.GetQuery<Domain.Entity.Registration>(_context);

                return new Tuple<long, ICollection<RegistrationModel>>
                    (await query.LongCountAsync(cancellationToken),
                     await query.Select(f => RegistrationModel.Create(f)).ToListAsync(cancellationToken)
                );
            }
        }
    }
}
