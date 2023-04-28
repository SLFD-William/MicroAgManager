using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class UpdateLandPlot : BaseCommand, IRequest<long>, IUpdateCommand
    {
        public LandPlotModel LandPlot { get; set; }

        public class Handler : IRequestHandler<UpdateLandPlot, long>
        {
            protected readonly IMicroAgManagementDbContext _context;
            protected readonly IMediator _mediator;
            public Handler(IMicroAgManagementDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }
            public async Task<long> Handle(UpdateLandPlot request, CancellationToken cancellationToken)
            {
                var farm = _context.Plots.First(f => f.TenantId == request.TenantId && f.Id == request.LandPlot.Id);
                farm = request.LandPlot.MapToEntity(farm);
                farm.ModifiedOn = DateTime.Now;
                farm.ModifiedBy = request.ModifiedBy;

                await _context.SaveChangesAsync(cancellationToken);
                //await _mediator.Publish(new NotificationMessage
                //{
                //    To = request.TenantId.ToString(),
                //    Body = $"{nameof(LandPlotModel)} {farm.Id}",
                //    From = request.ModifiedBy.ToString(),
                //    Subject = "Update"
                //}, cancellationToken);
                return farm.Id;
            }
        }
    }
}
