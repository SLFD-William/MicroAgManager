using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using FrontEnd.Components.Unit;
using BackEnd.BusinessLogic.TreatmentRecord;
using FrontEnd.Components.Treatment;

namespace FrontEnd.Components.TreatmentRecord
{
    public partial class TreatmentRecordEditor : HasRecipientComponent<TreatmentRecordModel>
    {
        [CascadingParameter] public TreatmentRecordModel TreatmentRecord { get; set; }
        [Parameter] public long? treatmentRecordId { get; set; }
        [Parameter] public required long treatmentId { get; set; }
        private ValidatedForm _validatedForm;

        #region Unit
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
           // ((TreatmentRecordModel)working).UnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void UnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        #endregion
        #region Treatment
        private TreatmentEditor _treatmentEditor;
        private bool showTreatmentModal = false;
        private void ShowTreatmentEditor()
        {
            showTreatmentModal = true;
            StateHasChanged();
        }
        private void TreatmentCreated(object e)
        {
            var model = e as TreatmentModel;
            showTreatmentModal = false;
            ((TreatmentRecordModel)working).TreatmentId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void TreatmentCanceled()
        {
            showTreatmentModal = false;
            StateHasChanged();
        }
        #endregion
        public override async Task FreshenData()
        {
            if (TreatmentRecord is not null)
                working = TreatmentRecord;

            if (TreatmentRecord is null && treatmentRecordId > 0)
                working = await app.dbContext.TreatmentRecords.FindAsync(treatmentRecordId);


            if (working is null && treatmentId>0)
            {
                var treatment = await app.dbContext.Treatments.FindAsync(treatmentId);
                working = new TreatmentRecordModel()
                {
                    RecipientId = RecipientId,
                    RecipientTypeId = RecipientTypeId,
                    RecipientType = RecipientType,
                    TreatmentId = treatmentId,
                    DosageUnitId = treatment.DosageUnitId ?? 0,
                    DosageAmount=treatment.DosageAmount,
                    AppliedMethod=treatment.LabelMethod,
                    DatePerformed = DateTime.Now,
                };
            }

            SetEditContext((TreatmentRecordModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (((TreatmentRecordModel)working).Id <= 0) ?
                     ((TreatmentRecordModel)working).Id = await app.api.ProcessCommand<TreatmentRecordModel, CreateTreatmentRecord>("api/CreateTreatmentRecord", new CreateTreatmentRecord { TreatmentRecord = (TreatmentRecordModel)working }) :
                     ((TreatmentRecordModel)working).Id = await app.api.ProcessCommand<TreatmentRecordModel, UpdateTreatmentRecord>("api/UpdateTreatmentRecord", new UpdateTreatmentRecord { TreatmentRecord = (TreatmentRecordModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                ((TreatmentRecordModel)working).Id = id;
                SetEditContext((TreatmentRecordModel)working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            working = original.Map((TreatmentRecordModel)working) ;
            SetEditContext((TreatmentRecordModel)working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
