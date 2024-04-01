using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Chore
{
    public class CreateChore : BaseCommand, ICreateCommand
    {
        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public ChoreModel Chore { get; set; }

        public class Handler:BaseCommandHandler<CreateChore>
        {
            public Handler(IMediator mediator, ILogger log) : base(mediator, log)
            {
            }

            public override async Task<long> Handle(CreateChore request, CancellationToken cancellationToken)
            {
                var chore = new Domain.Entity.Chore(request.ModifiedBy, request.TenantId);
                chore = request.Chore.Map(chore) as Domain.Entity.Chore;
                chore.ModifiedBy = chore.CreatedBy = request.ModifiedBy;
                chore.TenantId = request.TenantId;
                using (var context = new DbContextFactory().CreateDbContext())
                {
                    context.Chores.Add(chore);
                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                        await _mediator.Publish(new ModifiedEntityPushNotification(chore.TenantId, ChoreModel.Create(chore).GetJsonString(), nameof(ChoreModel)), cancellationToken);
                    }
                    catch (Exception ex) { _log.LogError(ex, "Unable to Create Chore"); }
                }
                return chore.Id;
            }
        }

    }
}
