using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Duty
{
    public partial class DutyList : DataComponent<DutyModel>
    {
        protected TableTemplate<DutySummary> _listComponent;
        [CascadingParameter] LivestockAnimalSummary LivestockAnimal { get; set; }
        [Parameter] public IEnumerable<DutySummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<DutyModel>? DutySelected { get; set; }

        private DutyModel? _editDuty;
        private DutyEditor? _DutyEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("DutyList"))
                app.RowDetailsShowing.Add("DutyList", new List<object>());
        }
        public override async Task FreshenData()
        {

            if (Items is null)
                Items = (await app.dbContext.Duties
                    .Where(f => f.RecipientTypeId == LivestockAnimal.Id && f.RecipientType == LivestockAnimal.EntityName).OrderBy(f => f.EntityModifiedOn)
                    .Select(s => new DutySummary(s, app.dbContext)).ToListAsync()).AsEnumerable();

            _listComponent?.Update();
        }
        private async Task<DutyModel?> FindDuty(long Id) => await app.dbContext.Duties.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                DutySelected?.Invoke(Task.Run(async () => await FindDuty(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditDuty(long id)
        {
            _editDuty = id > 0 ? await FindDuty(id) : new DutyModel();
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editDuty = null;
            StateHasChanged();
        }
        private async Task DutyUpdated(object args)
        {
            var model=args as DutyModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Duties.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }
            _editDuty = null;
            await Submitted.InvokeAsync(await FindDuty(model.Id));
        }
    }
}
