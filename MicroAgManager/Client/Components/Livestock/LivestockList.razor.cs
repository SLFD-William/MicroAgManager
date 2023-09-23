using Domain.Models;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockList : DataComponent
    {
        [CascadingParameter] LivestockBreedSummary LivestockBreed { get; set; }
        public TableTemplate<LivestockSummary> _listComponent;
        [Parameter] public IEnumerable<LivestockSummary> Items { get; set; } = new List<LivestockSummary>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockModel>? LivestockSelected { get; set; }
        private LivestockModel? _editLivestock;
        private LivestockEditor? _livestockEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("LivestockList"))
                app.RowDetailsShowing.Add("LivestockList", new List<object>());
        }
        private async Task<LivestockModel?> FindLivestock(long id)=> await app.dbContext.Livestocks.FindAsync(id);
        
        private async Task EditLivestock(long id)
        {
            _editLivestock = id > 0 ? await FindLivestock(id) : new LivestockModel { LivestockBreedId=LivestockBreed.Id};
            StateHasChanged();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                LivestockSelected?.Invoke(Task.Run(async()=> await  FindLivestock(_listComponent.SelectedItems.First().Id)).Result);
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            if (Items is null)
                Items = app.dbContext.Livestocks.Where(f => f.LivestockBreedId == LivestockBreed.Id).OrderBy(f => f.Name).Select(s=>new LivestockSummary(s,app.dbContext)).AsEnumerable() ?? new List<LivestockSummary>();
            
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
