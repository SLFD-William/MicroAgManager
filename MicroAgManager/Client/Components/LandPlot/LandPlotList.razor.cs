using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotList : DataComponent<LandPlotModel>
    {
        [CascadingParameter] FarmLocationModel Farm { get; set; }

        public TableTemplate<LandPlotSummary> _listComponent;
        [Parameter] public IEnumerable<LandPlotSummary> Items { get; set; }=new List<LandPlotSummary>();
        
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LandPlotModel>? PlotSelected { get; set; }

        private LandPlotModel? _editPlot;
        private LandPlotEditor? _landPlotEditor;

        private async Task<LandPlotModel?> FindPlot(long Id) => await app.dbContext.LandPlots.FindAsync(Id);
        private void TableItemSelected()
        { 
            if(_listComponent.SelectedItems.Count() > 0)
                PlotSelected?.Invoke(Task.Run(async () => await FindPlot(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditPlot(long id)
        {
            _editPlot = id > 0 ? await FindPlot(id) : new LandPlotModel {FarmLocationId=Farm.Id };
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editPlot = null;
            await FreshenData();
        }
        private async Task LandPlotUpdated(object args )
        {
            var model = args as LandPlotModel;
            if (model?.Id > 0)
                while (!app.dbContext.LandPlots.Any(t => t.Id == model.Id))
                    await Task.Delay(100);
            _editPlot = null;
            await FreshenData();
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            while (!app.dbContext.LandPlots.Any())
                await Task.Delay(100);

            if (Items is null)
                Items = app.dbContext.LandPlots.Where(f => f.FarmLocationId == Farm.Id).OrderBy(f => f.Usage).ThenBy(f=>f.Name).Select(f=>new LandPlotSummary(f,app.dbContext)).AsEnumerable() ?? new List<LandPlotSummary>();
            
            _listComponent.Update();
        }
    }
}
