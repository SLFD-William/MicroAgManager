using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Chore
{
    public class UpdateChore : BaseCommand, IUpdateCommand
    {
        public ChoreModel Chore { get; set; }
        public class Handler: BaseCommandHandler<UpdateChore>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateChore request, CancellationToken cancellationToken)
            {
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    var chore =await context.Chores.FirstAsync(d => d.TenantId == request.TenantId && d.Id == request.Chore.Id);
                    chore = request.Chore.Map(chore) as Domain.Entity.Chore;
                    await context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new ModifiedEntityPushNotification(chore.TenantId, ChoreModel.Create(chore).GetJsonString(), nameof(ChoreModel)), cancellationToken);
                    return chore.Id;
                }
            }
        }
    }
}
