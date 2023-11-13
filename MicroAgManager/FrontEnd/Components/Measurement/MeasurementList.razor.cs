using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Measurement
{
    public partial class MeasurementList : DataComponent<MeasurementModel>
    {
        public TableTemplate<MeasurementModel> _listComponent;
        [Parameter] public IEnumerable<MeasurementModel> Items { get; set; } = new List<MeasurementModel>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<MeasurementModel>? MeasurementSelected { get; set; }
        private MeasurementModel? _editMeasurement;
        private MeasurementEditor? _measurementEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("MeasurementList"))
                app.RowDetailsShowing.Add("MeasurementList", new List<object>());
        }
        private async Task<MeasurementModel?> FindMeasurement(long id) => await app.dbContext.Measurements.FindAsync(id);

        private async Task EditMeasurement(long id)
        {
            _editMeasurement = id > 0 ? await FindMeasurement(id) : new MeasurementModel { };
            StateHasChanged();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                MeasurementSelected?.Invoke(Task.Run(async () => await FindMeasurement(_listComponent.SelectedItems.First().Id)).Result);
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            if (Items is null)
                Items = app.dbContext.Measurements.OrderByDescending(f => f.DatePerformed).ToList() ?? new List<MeasurementModel>();

            _listComponent.Update();
        }
        private async Task EditCancelled()
        {
            _editMeasurement = null;
            StateHasChanged();
        }
        private async Task MeasurementUpdated(object args)
        {
            var model = args as MeasurementModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Measurements.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            _editMeasurement = null;
            await Submitted.InvokeAsync(await FindMeasurement(model.Id));
        }
    }
}
