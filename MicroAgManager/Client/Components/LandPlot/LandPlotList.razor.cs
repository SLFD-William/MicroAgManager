using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotList : DataComponent
    {
        [CascadingParameter] FarmLocationModel farm { get; set; }

        public TableTemplate<LandPlotModel> _listComponent;
        [Parameter] public IEnumerable<LandPlotModel> Items { get; set; }=new List<LandPlotModel>();
        
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LandPlotModel>? PlotSelected { get; set; }

        private LandPlotModel? _editPlot;
        private LandPlotEditor? _landPlotEditor;
        private void TableItemSelected()
        { 
            if(_listComponent.SelectedItems.Count() > 0)
                PlotSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        private void EditPlot(long id)
        {
            _editPlot = Items.First(p => p.Id == id);
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editPlot = null;
            await FreshenData();
        }
        private async Task LandPlotUpdated(LandPlotModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LandPlots.Any(t => t.Id == args.Id))
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
                Items = app.dbContext.LandPlots.Where(f => f.FarmLocationId == farm.Id).OrderBy(f => f.Usage).ThenBy(f=>f.Name).AsEnumerable() ?? new List<LandPlotModel>();
            
            _listComponent.Update();
        }
    }
}
