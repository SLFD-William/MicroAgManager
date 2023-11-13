using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.TreatmentRecord
{
    public partial class TreatmentRecordList : DataComponent<TreatmentRecordModel>
    {
        public TableTemplate<TreatmentRecordModel> _listComponent;
        [Parameter] public IEnumerable<TreatmentRecordModel> Items { get; set; } = new List<TreatmentRecordModel>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<TreatmentRecordModel>? TreatmentRecordSelected { get; set; }
        private TreatmentRecordModel? _editTreatmentRecord;
        private TreatmentRecordEditor? _treatmentRecordEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("TreatmentRecordList"))
                app.RowDetailsShowing.Add("TreatmentRecordList", new List<object>());
        }
        private async Task<TreatmentRecordModel?> FindTreatmentRecord(long id) => await app.dbContext.TreatmentRecords.FindAsync(id);

        private async Task EditTreatmentRecord(long id)
        {
            _editTreatmentRecord = id > 0 ? await FindTreatmentRecord(id) : new TreatmentRecordModel { };
            StateHasChanged();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                TreatmentRecordSelected?.Invoke(Task.Run(async () => await FindTreatmentRecord(_listComponent.SelectedItems.First().Id)).Result);
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            if (Items is null)
                Items = app.dbContext.TreatmentRecords.OrderByDescending(f => f.DatePerformed).ToList() ?? new List<TreatmentRecordModel>();

            _listComponent.Update();
        }
        private async Task EditCancelled()
        {
            _editTreatmentRecord = null;
            StateHasChanged();
        }
        private async Task TreatmentRecordUpdated(object args)
        {
            var model = args as TreatmentRecordModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.TreatmentRecords.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            _editTreatmentRecord = null;
            await Submitted.InvokeAsync(await FindTreatmentRecord(model.Id));
        }
    }
}
