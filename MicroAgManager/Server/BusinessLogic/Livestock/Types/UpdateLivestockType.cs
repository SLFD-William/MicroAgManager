using BackEnd.Abstracts;
using BackEnd.Infrastructure;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class UpdateLivestockType : BaseCommand, IUpdateCommand
    {
        public Domain.Models.LivestockTypeModel LivestockType { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestockType>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator, ILogger log) : base(context, mediator, log)
            {
            }

            public override async Task<long> Handle(UpdateLivestockType request, CancellationToken cancellationToken)
            {
                var livestockType = _context.LivestockTypes.Find(request.LivestockType.Id);
                livestockType = request.LivestockType.MapToEntity(livestockType);
                livestockType.ModifiedOn = DateTime.Now;
                livestockType.ModifiedBy = request.ModifiedBy;
                livestockType.TenantId = request.TenantId;
                _context.LivestockTypes.Update(livestockType);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockType.Id.ToString(), livestockType.GetType().Name, "Modified", livestockType.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { _log.LogError(ex, "Unable to Update Livestock Type"); }
                return livestockType.Id;
            }
        }
    }
}
