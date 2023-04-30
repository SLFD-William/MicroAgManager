using BackEnd.Abstracts;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.BusinessLogic.Livestock.Types
{
    public class UpdateLivestockType : BaseCommand, IUpdateCommand
    {
        public Guid ModifiedBy { get; set; }
        public Guid TenantId { get; set; }
        public Domain.Models.LivestockTypeModel LivestockType { get; set; }
        public class Handler : BaseCommandHandler<UpdateLivestockType>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
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
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                //await _mediator.Publish(new NotificationMessage
                //{
                //    To = request.TenantId.ToString(),
                //    Body = $"{nameof(LandPlotModel)} {farm.Id}",
                //    From = request.ModifiedBy.ToString(),
                //    Subject = "Create"
                //}, cancellationToken);
                return livestockType.Id;
            }
        }
    }
}
