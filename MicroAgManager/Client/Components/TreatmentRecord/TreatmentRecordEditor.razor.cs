using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using FrontEnd.Components.Unit;
using BackEnd.BusinessLogic.TreatmentRecord;

namespace FrontEnd.Components.TreatmentRecord
{
    public partial class TreatmentRecordEditor : DataComponent<TreatmentRecordModel>
    {
        [CascadingParameter] public TreatmentRecordModel TreatmentRecord { get; set; }
        [Parameter] public long? treatmentRecordId { get; set; }
        [Parameter] public required long treatmentId { get; set; }
        [Parameter] public required long recipientTypeId { get; set; }
        [Parameter] public required long recipientId { get; set; }
        [Parameter] public required string recipientType { get; set; }

        private ValidatedForm _validatedForm;

        protected new TreatmentRecordModel working { get => base.working as TreatmentRecordModel; set { base.working = value; } }
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


            working = new TreatmentRecordModel() { };

            if (TreatmentRecord is not null)
                working = TreatmentRecord;

            if (TreatmentRecord is null && treatmentRecordId > 0)
                working = await app.dbContext.TreatmentRecords.FindAsync(treatmentRecordId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (working.Id <= 0) ?
                     working.Id = await app.api.ProcessCommand<TreatmentRecordModel, CreateTreatmentRecord>("api/CreateTreatmentRecord", new CreateTreatmentRecord { TreatmentRecord = working }) :
                     working.Id = await app.api.ProcessCommand<TreatmentRecordModel, UpdateTreatmentRecord>("api/UpdateTreatmentRecord", new UpdateTreatmentRecord { TreatmentRecord = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                working.Id = id;
                original = working.Clone() as TreatmentRecordModel;
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
            working = ((TreatmentRecordModel)original).Map(working) as TreatmentRecordModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
