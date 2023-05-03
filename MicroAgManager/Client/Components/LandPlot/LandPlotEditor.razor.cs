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
    public partial class LandPlotEditor:ComponentBase, IAsyncDisposable
    {
        [CascadingParameter] FarmLocationModel farm { get; set; }
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public bool createOnly { get; set; } 
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
            if (_submitting) return;
            if (dbContext is null)
                dbContext = await dbSync.GetPreparedDbContextAsync();
            var query = dbContext.LandPlots.AsQueryable();
            if (farm is not null) query = query.Where(f => f.FarmLocationId==farm.Id);
            if (parentPlotId.HasValue && parentPlotId > 0)
                query = query.Where(f => f.ParentPlotId == parentPlotId);
            if (landPlotId.HasValue && landPlotId>0)
                query = query.Where(f => f.Id == landPlotId);
            plot= new LandPlotModel();
            if(!createOnly)
                plot = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LandPlotModel();
            ApplyRelatedIds();
            editContext = new EditContext(plot);
        }
        private void ApplyRelatedIds()
        {
            if (farm is not null) plot.FarmLocationId = farm.Id;
            if (parentPlotId.HasValue && parentPlotId > 0) plot.ParentPlotId = parentPlotId;

        }
        private bool _submitting = false;
        public async Task OnSubmit()
        {
            _submitting = true;
            var id = plot.Id;
            if (id <= 0)
                id = await api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = plot });
            else
                id = await api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = plot }); 

            if (id <= 0)
                throw new Exception("Unable to save plot");
            plot.Id=id;
            editContext = new EditContext(plot);
            editContext.MarkAsUnmodified();
            await Submitted.InvokeAsync(plot);
            _submitting = false;
            if (createOnly)
            {
                plot = new LandPlotModel();
                ApplyRelatedIds();
                editContext = new EditContext(plot);
                editContext.MarkAsUnmodified();
                await Submitted.InvokeAsync(plot);
            }
               
            StateHasChanged();
        }
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }

    }
}
