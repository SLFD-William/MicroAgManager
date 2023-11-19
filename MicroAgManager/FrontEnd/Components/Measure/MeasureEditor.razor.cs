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
            ((MeasureModel)working).UnitId = model.Id;
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
            working = new MeasureModel();
            if (Measure is not null)
                working = Measure;

            if (Measure is null && measureId > 0)
                working = await app.dbContext.Measures.FindAsync(measureId);

            SetEditContext((MeasureModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (((MeasureModel)working).Id <= 0) ?
                     await app.api.ProcessCommand<MeasureModel, CreateMeasure>("api/CreateMeasure", new CreateMeasure { Measure = (MeasureModel)working }):
                     await app.api.ProcessCommand<MeasureModel, UpdateMeasure>("api/UpdateMeasure", new UpdateMeasure { Measure = (MeasureModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                ((MeasureModel)working).Id = id;
                SetEditContext((MeasureModel)working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task Cancel()
        {
            working = original.Map((MeasureModel)working);
            SetEditContext((MeasureModel)working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
