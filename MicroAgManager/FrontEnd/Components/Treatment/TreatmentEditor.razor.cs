using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Treatment;
using FrontEnd.Components.Unit;

namespace FrontEnd.Components.Treatment
{
    public partial class TreatmentEditor : DataComponent<TreatmentModel>
    {
        [CascadingParameter] public TreatmentModel Treatment { get; set; }
        [Parameter] public long? TreatmentId { get; set; }
        private ValidatedForm _validatedForm;

        private UnitEditor _unitEditor;
        private bool showUnitModal = false;
        private void ShowUnitEditor()
        {
            showUnitModal = true;
            StateHasChanged();
        }
        private void DosageUnitCreated(object e)
        {
            var model = e as UnitModel;
            showUnitModal = false;
            ((TreatmentModel)working).DosageUnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void DosageUnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        private void DurationUnitCreated(object e)
        {
            var model = e as UnitModel;
            showUnitModal = false;
            ((TreatmentModel)working).DurationUnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void DurationUnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        private void FrequencyUnitCreated(object e)
        {
            var model = e as UnitModel;
            showUnitModal = false;
            ((TreatmentModel)working).FrequencyUnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void FrequencyUnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        private void MassUnitCreated(object e)
        {
            var model = e as UnitModel;
            showUnitModal = false;
            ((TreatmentModel)working).RecipientMassUnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void MassUnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        public override async Task FreshenData()
        {

            working = new TreatmentModel() { };

            if (Treatment is not null)
                working = Treatment;

            if (Treatment is null && TreatmentId > 0)
                working = await app.dbContext.Treatments.FindAsync(TreatmentId);

            SetEditContext((TreatmentModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (((TreatmentModel)working).Id <= 0) ?
                     ((TreatmentModel)working).Id = await app.api.ProcessCommand<TreatmentModel, CreateTreatment>("api/CreateTreatment", new CreateTreatment { Treatment = (TreatmentModel)working }) :
                     ((TreatmentModel)working).Id = await app.api.ProcessCommand<TreatmentModel, UpdateTreatment>("api/UpdateTreatment", new UpdateTreatment { Treatment = (TreatmentModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                ((TreatmentModel)working).Id = id;
                original = ((TreatmentModel)working).Clone() as TreatmentModel;
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
            working = original.Map((TreatmentModel)working);
            SetEditContext((TreatmentModel)working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
