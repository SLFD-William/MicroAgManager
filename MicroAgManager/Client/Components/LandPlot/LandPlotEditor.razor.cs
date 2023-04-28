using BackEnd.BusinessLogic.LandPlots;
using Domain.Enums;
using Domain.Models;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotEditor
    {
        [CascadingParameter] FarmLocationModel farm { get; set; }
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public long? landPlotId { get; set; }
        [Parameter] public long? parentPlotId { get; set; }
        [Parameter] public EventCallback<LandPlotModel> Submitted { get; set; }
        LandPlotModel plot { get; set; }
        public EditContext editContext { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            var query = dbContext.LandPlots.AsQueryable();
            if (landPlotId.HasValue && landPlotId>0)
                query = query.Where(f => f.Id == landPlotId);
            plot = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LandPlotModel() {FarmLocationId=farm.Id};
            editContext = new EditContext(plot);
        }
        //private long areaUnit
        //{
        //    get => plot.AreaUnit?.Id ?? 0;
        //    set
        //    {
        //        plot.AreaUnit = value == 0 ? null : BaseEnumeration.FromId<UnitEnum>(value);
        //    }
        //}
        //private long usage
        //{
        //    get => plot.Usage?.Id ?? 0;
        //    set
        //    {
        //        plot.Usage = value == 0 ? null : BaseEnumeration.FromId<PlotUseEnum>(value);
        //    }
        //}
        public async Task OnSubmit()
        {
            try
            {
                var state = plot.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (state == EntityState.Added)
                    plot.Id = await api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = plot });
                else
                    plot.Id = await api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = plot }); 

                dbContext.Attach(plot);
                dbContext.Entry(plot).State = state;
                await dbContext.SaveChangesAsync();
                editContext = new EditContext(plot);
                await Submitted.InvokeAsync(plot);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
