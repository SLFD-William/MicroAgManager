using BackEnd.BusinessLogic.LandPlots;
using Domain.Models;
using FrontEnd.Data;
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
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public long? landPlotId { get; set; }
        [Parameter] public long? parentPlotId { get; set; }
        [Parameter] public EventCallback<LandPlotModel> Submitted { get; set; }
        LandPlotModel plot { get; set; }
        public EditContext editContext { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            dbSync.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
        {
            if (dbContext is null)
                dbContext = await dbSync.GetPreparedDbContextAsync();
            var query = dbContext.LandPlots.AsQueryable();
            if (landPlotId.HasValue && landPlotId>0)
                query = query.Where(f => f.Id == landPlotId);
            plot = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LandPlotModel() {FarmLocationId=farm.Id};
            editContext = new EditContext(plot);
        }
        public async Task OnSubmit()
        {
            try
            {

                if (plot.Id <= 0)
                    plot.Id = await api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = plot });
                else
                    plot.Id = await api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = plot }); 

                if (plot.Id <= 0)
                    throw new Exception("Unable to save plot");

                editContext = new EditContext(plot);
                await Submitted.InvokeAsync(plot);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }

    }
}
