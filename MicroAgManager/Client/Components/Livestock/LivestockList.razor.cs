using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockList : DataComponent
    {
        [CascadingParameter] LivestockBreedModel LivestockBreed { get; set; }
        public TableTemplate<LivestockModel> _listComponent;
        [Parameter] public IEnumerable<LivestockModel> Items { get; set; } = new List<LivestockModel>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockModel>? LivestockSelected { get; set; }
        private LivestockModel? _editLivestock;
        private LivestockEditor? _livestockEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("LivestockList"))
                app.RowDetailsShowing.Add("LivestockList", new List<object>());
        }
        private void EditLivestock(long id)
        {
            _editLivestock = id > 0 ? Items.First(p => p.Id == id) : new LivestockModel { LivestockBreedId=LivestockBreed.Id};
            StateHasChanged();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                LivestockSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            if (Items is null)
                Items = app.dbContext.Livestocks.Where(f => f.LivestockBreedId == LivestockBreed.Id).OrderBy(f => f.Name).AsEnumerable() ?? new List<LivestockModel>();
            
            _listComponent.Update();
        }
        private async Task EditCancelled()
        {
            _editLivestock = null;
            await FreshenData();
        }
        private async Task LivestockUpdated(LivestockModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.Livestocks.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editLivestock = null;
            await FreshenData();
        }
    }
}
