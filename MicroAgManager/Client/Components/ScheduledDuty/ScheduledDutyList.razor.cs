using Domain.Models;
using FrontEnd.Components.Shared.Sortable;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.ScheduledDuty
{
    public partial class ScheduledDutyList : DataComponent<ScheduledDutyModel>
    {
        public TableTemplate<ScheduledDutySummary> _listComponent;
        [Parameter] public IEnumerable<ScheduledDutySummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<ScheduledDutyModel>? ScheduledDutySelected { get; set; }

        private ScheduledDutyModel? _editScheduledDuty;
        private ScheduledDutyEditor? _scheduledDutyEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("ScheduledDutyList"))
                app.RowDetailsShowing.Add("ScheduledDutyList", new List<object>());
        }

        private async Task<ScheduledDutyModel?> FindScheduledDuty(long Id) => await app.dbContext.ScheduledDuties.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                ScheduledDutySelected?.Invoke(Task.Run(async () => await FindScheduledDuty(_listComponent.SelectedItems.First().Id)).Result);
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = await app.dbContext.ScheduledDuties.OrderByDescending(f => f.DueOn).Select(s => new ScheduledDutySummary(s, app.dbContext)).ToListAsync();

            StateHasChanged();
            _listComponent.Update();
        }
        private async Task EditCancelled()
        {
            _editScheduledDuty = null;
            await FreshenData();
        }
        private async Task ScheduledDutyUpdated(object args)
        {
            var model = args as ScheduledDutyModel;
            if (model?.Id > 0)
                while (!app.dbContext.ScheduledDuties.Any(t => t.Id == model.Id))
                    await Task.Delay(100);

            _editScheduledDuty = null;
            await FreshenData();
        }
        private async Task EditScheduledDuty(long id)
        {
            _editScheduledDuty = id > 0 ? await FindScheduledDuty(id) : new ScheduledDutyModel();
            StateHasChanged();
        }
    }
}
