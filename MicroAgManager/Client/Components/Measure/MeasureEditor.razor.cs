using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Measure;
using FrontEnd.Components.Unit;

namespace FrontEnd.Components.Measure
{
    public partial class MeasureEditor : DataComponent<MeasureModel>
    {
        [CascadingParameter] public MeasureModel Measure { get; set; }
        [Parameter] public long? measureId { get; set; }
        private ValidatedForm _validatedForm;

        protected new MeasureModel working { get => base.working as MeasureModel; set { base.working = value; } }
        private UnitEditor _unitEditor;
        private bool showUnitModal = false;
        private void ShowUnitEditor()
        {
            showUnitModal = true;
            StateHasChanged();
        }
        private void UnitCreated(object e)
        {
            var model = e as UnitModel;
            showUnitModal = false;
            working.UnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void UnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        public override async Task FreshenData()
        {
            if (Measure is not null)
                working = Measure;

            if (Measure is null && measureId > 0)
                working = await app.dbContext.Measures.FindAsync(measureId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (working.Id <= 0) ?
                     await app.api.ProcessCommand<MeasureModel, CreateMeasure>("api/CreateMeasure", new CreateMeasure { Measure = working }):
                     await app.api.ProcessCommand<MeasureModel, UpdateMeasure>("api/UpdateMeasure", new UpdateMeasure { Measure = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task Cancel()
        {
            working = original.Map(working) as MeasureModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
