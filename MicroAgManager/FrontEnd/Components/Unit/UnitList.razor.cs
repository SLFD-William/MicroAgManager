using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Unit
{
    public partial class UnitList: DataComponent<UnitModel>
    {
        protected TableTemplate<UnitModel> _listComponent;
        [Parameter] public IEnumerable<UnitModel>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<UnitModel>? UnitSelected { get; set; }

        private UnitModel? _editUnit;
        private UnitEditor? _unitEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("UnitList"))
                app.RowDetailsShowing.Add("UnitList", new List<object>());
        }
        public override async Task FreshenData()
        {

            if (Items is null)
                Items = (await app.dbContext.Units
                    .OrderBy(f => f.EntityModifiedOn)
                    .ToListAsync()).AsEnumerable();

            _listComponent?.Update();
        }
        private async Task<UnitModel?> FindUnit(long Id) => await app.dbContext.Units.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                UnitSelected?.Invoke(Task.Run(async () => await FindUnit(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditUnit(long id)
        {
            _editUnit = id > 0 ? await FindUnit(id) : new UnitModel { Category="", ConversionFactorToSIUnit=0, Name="", Symbol="" };
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editUnit = null;
            StateHasChanged();
        }
        private async Task UnitUpdated(object args)
        {
            var model = args as UnitModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Units.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            _editUnit = null;
            await Submitted.InvokeAsync(await FindUnit(model.Id));
        }
    }
}
