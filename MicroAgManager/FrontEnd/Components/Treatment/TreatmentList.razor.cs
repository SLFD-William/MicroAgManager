using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Treatment
{
    public partial class TreatmentList : DataComponent<TreatmentModel>
    {
        protected TableTemplate<TreatmentSummary> _listComponent;
        [Parameter] public IEnumerable<TreatmentSummary>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<TreatmentModel>? TreatmentSelected { get; set; }

        private TreatmentModel? _editTreatment;
        private TreatmentEditor? _treatmentEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("TreatmentList"))
                app.RowDetailsShowing.Add("TreatmentList", new List<object>());
        }
        public override async Task FreshenData()
        {

            if (Items is null)
                Items = (await app.dbContext.Treatments
                    .OrderBy(f => f.EntityModifiedOn).Select(m=> new TreatmentSummary(m,app.dbContext))
                    .ToListAsync()).AsEnumerable();

            _listComponent?.Update();
        }
        private async Task<TreatmentModel?> FindTreatment(long Id) => await app.dbContext.Treatments.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                TreatmentSelected?.Invoke(Task.Run(async () => await FindTreatment(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditTreatment(long id)
        {
            _editTreatment = id > 0 ? await FindTreatment(id) : new TreatmentModel();
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editTreatment = null;
            StateHasChanged();
        }
        private async Task TreatmentUpdated(object args)
        {
            var model = args as TreatmentModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Treatments.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            _editTreatment = null;
            await Submitted.InvokeAsync(await FindTreatment(model.Id));
        }
    }
}
