using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusList:DataComponent
    {
        protected TableTemplate<LivestockStatusSummary> _listComponent;
        [CascadingParameter] LivestockAnimalSummary LivestockAnimal { get; set; }
        [Parameter] public IEnumerable<LivestockStatusSummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<LivestockStatusModel>? StatusSelected { get; set; }

        private LivestockStatusModel? _editStatus;
        private LivestockStatusEditor? _statusEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("LivestockStatusList"))
                app.RowDetailsShowing.Add("LivestockStatusList", new List<object>());
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            
            if (Items is null)
                Items = (await app.dbContext.LivestockStatuses
                    .Where(f => f.LivestockAnimalId == LivestockAnimal.Id).OrderBy(f => f.EntityModifiedOn)
                    .Select(s=>new LivestockStatusSummary(s,app.dbContext)).ToListAsync()).AsEnumerable();

            _listComponent.Update();
        }
        private async Task<LivestockStatusModel?> FindStatus(long Id) => await app.dbContext.LivestockStatuses.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                StatusSelected?.Invoke(Task.Run(async()=> await FindStatus(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task  EditStatus(long id)
        {
            _editStatus = id > 0 ? await FindStatus(id) : new LivestockStatusModel { LivestockAnimalId= LivestockAnimal.Id };
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editStatus = null;
            await FreshenData();
        }
        private async Task StatusUpdated(LivestockStatusModel args)
        {
            if (args.Id > 0)
                while (!app.dbContext.LivestockStatuses.Any(t => t.Id == args.Id))
                    await Task.Delay(100);

            _editStatus = null;
            await FreshenData();
        }
    }
}
