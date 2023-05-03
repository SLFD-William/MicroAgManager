using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace BackEnd.BusinessLogic.Livestock.Status
{
    public class UpdateLivestockStatus:BaseCommand,IUpdateCommand
    {
        public Guid ModifiedBy { get; set; }
        public Guid TenantId { get; set; }
        public Domain.Models.LivestockStatusModel LivestockStatus { get; set; }

        public class Handler : BaseCommandHandler<UpdateLivestockStatus>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }
            public override async Task<long> Handle(UpdateLivestockStatus request, CancellationToken cancellationToken)
            {
                var livestockStatus = _context.LivestockStatuses.Find(request.LivestockStatus.Id);
                livestockStatus = request.LivestockStatus.MapToEntity(livestockStatus);
                livestockStatus.ModifiedOn = DateTime.Now;
                livestockStatus.ModifiedBy = request.ModifiedBy;
                livestockStatus.TenantId = request.TenantId;
                _context.LivestockStatuses.Update(livestockStatus);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(livestockStatus.Id.ToString(), livestockStatus.GetType().Name, "Modified", livestockStatus.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return livestockStatus.Id;
            }
        }

    }
}
