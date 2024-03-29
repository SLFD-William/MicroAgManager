﻿using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using FrontEnd.Components.Unit;
using BackEnd.BusinessLogic.Measurement;
using FrontEnd.Components.Measure;
using Domain.Abstracts;

namespace FrontEnd.Components.Measurement
{
    public partial class MeasurementEditor : HasRecipientComponent<MeasurementModel>
    {
        [CascadingParameter] public MeasurementModel Measurement { get; set; }
        [Parameter] public long? measurementId { get; set; }
        [Parameter] public required long measureId { get; set; }
        

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
           // ((MeasurementModel)working).UnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void UnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        #endregion
        #region Measure
        private bool showMeasureModal = false;
        private MeasureEditor _measureEditor;
        private void ShowMeasureEditor()
        {
            showMeasureModal = true;
            StateHasChanged();
        }
        private void MeasureCanceled()
        {
            ((MeasurementModel)working).MeasureId = ((MeasurementModel)original).MeasureId;
            showMeasureModal = false;
            StateHasChanged();
        }
        private void MeasureCreated(object e)
        {
            var status = e as BaseModel;
            showMeasureModal = false;
            ((MeasurementModel)working).MeasureId = status?.Id ?? 0;
            StateHasChanged();
        }
        #endregion
        public override async Task FreshenData()
        {
            if (Measurement is not null)
                working = Measurement;

            if (Measurement is null && measurementId > 0)
                working = await app.dbContext.Measurements.FindAsync(measurementId);

            if(working is null && measureId>0 )
            {
                var measure = await app.dbContext.Measures.FindAsync(measureId);
                working = new MeasurementModel()
                {
                    RecipientId = RecipientId,
                    RecipientTypeId = RecipientTypeId,
                    RecipientType = RecipientType,
                    MeasureId = measureId,
                    MeasurementUnitId = measure?.UnitId ?? 0,
                    DatePerformed = DateTime.Now,
                };
            }
            SetEditContext((MeasurementModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (((MeasurementModel)working).Id <= 0) ?
                     await app.api.ProcessCommand<MeasurementModel, CreateMeasurement>("api/CreateMeasurement", new CreateMeasurement { Measurement = (MeasurementModel)working }) :
                     await app.api.ProcessCommand<MeasurementModel, UpdateMeasurement>("api/UpdateMeasurement", new UpdateMeasurement { Measurement = (MeasurementModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                ((MeasurementModel)working).Id = id;
                SetEditContext((MeasurementModel)working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task Cancel()
        {
            working = original.Map((MeasurementModel)working);
            SetEditContext((MeasurementModel)working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
