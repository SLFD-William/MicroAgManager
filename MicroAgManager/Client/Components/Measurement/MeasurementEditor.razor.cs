using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using FrontEnd.Components.Unit;
using BackEnd.BusinessLogic.Measurement;

namespace FrontEnd.Components.Measurement
{
    public partial class MeasurementEditor : DataComponent<MeasurementModel>
    {
        [CascadingParameter] public MeasurementModel Measurement { get; set; }
        [Parameter] public long? measurementId { get; set; }
        private ValidatedForm _validatedForm;

        protected new MeasurementModel working { get => base.working as MeasurementModel; set { base.working = value; } }
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
           // working.UnitId = model.Id;
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


            working = new MeasurementModel() { };

            if (Measurement is not null)
                working = Measurement;

            if (Measurement is null && measurementId > 0)
                working = await app.dbContext.Measurements.FindAsync(measurementId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (working.Id <= 0) ?
                     working.Id = await app.api.ProcessCommand<MeasurementModel, CreateMeasurement>("api/CreateMeasurement", new CreateMeasurement { Measurement = working }) :
                     working.Id = await app.api.ProcessCommand<MeasurementModel, UpdateMeasurement>("api/UpdateMeasurement", new UpdateMeasurement { Measurement = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                working.Id = id;
                original = working.Clone() as MeasurementModel;
                editContext = new EditContext(working);
                await Submitted.InvokeAsync(working);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task Cancel()
        {
            working = ((MeasurementModel)original).Map(working) as MeasurementModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
