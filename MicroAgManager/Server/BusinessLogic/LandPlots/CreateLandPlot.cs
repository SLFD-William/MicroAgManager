using BackEnd.Abstracts;
using BackEnd.Models;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.LandPlots
{
    public class CreateLandPlot : BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required] public LandPlotModel LandPlot { get; set; }
        public class Handler : BaseCommandHandler<CreateLandPlot>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }

            public override async Task<long> Handle(CreateLandPlot request, CancellationToken cancellationToken)
            {
                var plot = new Domain.Entity.LandPlot(request.ModifiedBy, request.TenantId);
                plot = request.LandPlot.MapToEntity(plot);
                plot.FarmLocation = _context.Farms.Find(plot.FarmLocationId);

                plot.ModifiedOn = plot.CreatedOn = DateTime.Now;
                plot.ModifiedBy = plot.CreatedBy = request.ModifiedBy;
                plot.TenantId = request.TenantId;

                _context.Plots.Add(plot);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _mediator.Publish(new EntitiesModifiedNotification(request.TenantId, new() { new ModifiedEntity(plot.Id.ToString(), plot.GetType().Name, "Created", plot.ModifiedBy) }), cancellationToken);
                }
                catch (Exception ex) {Console.WriteLine(ex.ToString());}
                return plot.Id;
            }

        }

    }
}
