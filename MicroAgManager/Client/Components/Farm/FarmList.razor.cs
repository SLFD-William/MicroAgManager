using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Farm
{
    public partial class FarmList:DataComponent
    {
        public TableTemplate<FarmLocationModel> _listComponent;
        [Parameter] public IEnumerable<FarmLocationModel>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<FarmLocationModel>? FarmSelected { get; set; }


        private FarmLocationModel? _editFarm;
        private FarmEditor? _farmEditor;

        private void EditFarm(long id)
        {
            _editFarm = Items.First(p => p.Id == id);
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editFarm = null;
            await FreshenData();
        }
        private async Task FarmUpdated(FarmLocationModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.Farms.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editFarm = null;
            await FreshenData();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                FarmSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = app.dbContext.Farms.OrderBy(f => f.Name).AsEnumerable();

            StateHasChanged();
            _listComponent.Update();
        }

    }
}
