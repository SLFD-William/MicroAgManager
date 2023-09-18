using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock
{
    public class UpdateLivestock : BaseCommand, IUpdateCommand
    {
        public Domain.Models.LivestockModel Livestock { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestock>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestock request, CancellationToken cancellationToken)
            {
                var livestock = _context.Livestocks.Find(request.Livestock.Id);
                livestock = request.Livestock.MapToEntity(livestock);
                livestock.ModifiedOn = DateTime.Now;
                livestock.ModifiedBy = request.ModifiedBy;
                livestock.TenantId = request.TenantId;
                _context.Livestocks.Update(livestock);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestock.Id.ToString(), livestock.GetType().Name, "Modified", livestock.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock"); }
                return livestock.Id;
            }
        }
    }
}
