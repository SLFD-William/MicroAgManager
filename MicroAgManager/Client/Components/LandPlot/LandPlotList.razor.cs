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

        private void TableItemSelected()
        { 
            if(_listComponent.SelectedItems.Count() > 0)
                PlotSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (!Items.Any())
                Items = app?.dbContext?.LandPlots.Where(f => f.FarmLocationId == farm.Id).OrderBy(f => f.ModifiedOn).AsEnumerable() ?? new List<LandPlotModel>();

            StateHasChanged();
            _listComponent.Update();
        }
    }
}
