﻿using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Treatment;
using FrontEnd.Components.Unit;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Treatment
{
    public partial class TreatmentEditor : DataComponent<TreatmentModel>
    {
        [CascadingParameter] public TreatmentModel Treatment { get; set; }
        [Parameter] public long? TreatmentId { get; set; }
        private ValidatedForm _validatedForm;

        protected new TreatmentModel working { get => base.working as TreatmentModel; set { base.working = value; } }
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
            working.DosageUnitId = model.Id;
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
            working.DurationUnitId = model.Id;
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
            working.FrequencyUnitId = model.Id;
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
            working.RecipientMassUnitId = model.Id;
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



            if (Treatment is not null)
                working = Treatment;

            if (Treatment is null && TreatmentId > 0)
                working = await app.dbContext.Treatments.FindAsync(TreatmentId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (working.Id <= 0) ?
                     working.Id = await app.api.ProcessCommand<TreatmentModel, CreateTreatment>("api/CreateTreatment", new CreateTreatment { Treatment = working }) :
                     working.Id = await app.api.ProcessCommand<TreatmentModel, UpdateTreatment>("api/UpdateTreatment", new UpdateTreatment { Treatment = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                working.Id = id;
                original = working.Clone() as TreatmentModel;
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
            working = ((TreatmentModel)original).Map(working) as TreatmentModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
