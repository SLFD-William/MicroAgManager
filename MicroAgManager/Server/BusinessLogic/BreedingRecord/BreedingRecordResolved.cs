using BackEnd.Abstracts;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class BreedingRecordResolved : BaseNotification
    {
        public class BreedingRecordResolvedHandler : BaseNotificationHandler<BreedingRecordResolved>
        {
            public BreedingRecordResolvedHandler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task Handle(BreedingRecordResolved notification, CancellationToken cancellationToken)
            {
                var createLivestocks = await LivestockLogic.OnBreedingRecordResolved(_context, notification.Id, cancellationToken);
                foreach(var livestock in createLivestocks)
                    await _mediator.Send(livestock, cancellationToken);
            }
        }
    }
}
