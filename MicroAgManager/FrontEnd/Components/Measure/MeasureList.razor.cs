using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Measure
{
    public partial class MeasureList : DataComponent<MeasureModel>
    {
        protected TableTemplate<MeasureSummary> _listComponent;
        [Parameter] public IEnumerable<MeasureSummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<MeasureModel>? MeasureSelected { get; set; }

        private MeasureModel? _editMeasure;
        private MeasureEditor? _measureEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("MeasureList"))
                app.RowDetailsShowing.Add("MeasureList", new List<object>());
        }
        public override async Task FreshenData()
        {
            if (Items is null)
                Items = (await app.dbContext.Measures
                    .OrderBy(f => f.Method).ThenBy(f=>f.Name).Select(m=> new MeasureSummary(m,app.dbContext))
                    .ToListAsync()).AsEnumerable();

            _listComponent?.Update();
        }
        private async Task<MeasureModel?> FindMeasure(long Id) => await app.dbContext.Measures.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                MeasureSelected?.Invoke(Task.Run(async () => await FindMeasure(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditMeasure(long id)
        {
            _editMeasure = id > 0 ? await FindMeasure(id) : new MeasureModel();
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editMeasure = null;
            StateHasChanged();
        }
        private async Task MeasureUpdated(object args)
        {
            var model = args as MeasureModel;
            if (model?.Id > 0)
                while (!app.dbContext.Measures.Any(t => t.Id == model.Id))
                    await Task.Delay(100);

            _editMeasure = null;
            await Submitted.InvokeAsync(await FindMeasure(model.Id));
        }
    }
}
