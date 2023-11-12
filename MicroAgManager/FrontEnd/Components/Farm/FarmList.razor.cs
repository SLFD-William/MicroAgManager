using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Farm
{
    public partial class FarmList:DataComponent<FarmLocationModel>
    {
        public TableTemplate<FarmLocationSummary> _listComponent;
        [Parameter] public IEnumerable<FarmLocationSummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<FarmLocationModel>? FarmSelected { get; set; }
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("FarmList"))
                app.RowDetailsShowing.Add("FarmList", new List<object>());
        }

        private FarmLocationModel? _editFarm;
        private FarmEditor? _farmEditor;
        private async Task<FarmLocationModel?> FindFarm(long Id) =>await  app.dbContext.Farms.FindAsync(Id);
        private async Task EditFarm(long id)
        {
            
            _editFarm = id > 0 ? await FindFarm(id) : new FarmLocationModel();
            StateHasChanged();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                FarmSelected?.Invoke(Task.Run(async()=> await FindFarm(_listComponent.SelectedItems.First().Id)).Result);
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = app.dbContext.Farms.OrderBy(f => f.Name).Select(f => new FarmLocationSummary(f, app.dbContext)).AsEnumerable();

            _listComponent.Update();

        }
        private async Task EditCancelled()
        {
            _editFarm = null;
            StateHasChanged();
        }
        private async Task FarmUpdated(object args)
        {
            var model=args as FarmLocationModel;

            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Farms.Any(t => t.Id == model.Id))
                { 
                    await Task.Delay(100);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }
            _editFarm = null;
            await Submitted.InvokeAsync(await FindFarm(model.Id));
        }
    }
}
