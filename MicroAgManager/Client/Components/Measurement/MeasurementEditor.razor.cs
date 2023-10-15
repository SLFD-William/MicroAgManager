using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using FrontEnd.Components.Unit;
using BackEnd.BusinessLogic.Measurement;
using Domain.Constants;
using FrontEnd.Components.Measure;
using Domain.Abstracts;

namespace FrontEnd.Components.Measurement
{
    public partial class MeasurementEditor : DataComponent<MeasurementModel>
    {
        [CascadingParameter] public MeasurementModel Measurement { get; set; }
        [Parameter] public long? measurementId { get; set; }
        [Parameter] public required long measureId { get; set; }
        [Parameter] public required long  recipientTypeId { get; set; }
        [Parameter] public required long recipientId { get; set; }
        [Parameter] public required string recipientType { get; set; }


        private ValidatedForm _validatedForm;
        private MeasureEditor _measureEditor;
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
        private List<KeyValuePair<long, string>> recipientTypeIds()
        {
            switch (working.RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.LivestockAnimals.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private List<KeyValuePair<long, string>> recipientIds()
        {
            switch (working.RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private bool showMeasureModal = false;

        private void ShowMeasureEditor()
        {
            showMeasureModal = true;
            StateHasChanged();
        }
        private void MeasureCanceled()
        {
            working.MeasureId = ((MeasurementModel)original).MeasureId;
            showMeasureModal = false;
            StateHasChanged();
        }
        private void MeasureCreated(object e)
        {
            var status = e as BaseModel;
            showMeasureModal = false;
            working.MeasureId = status?.Id ?? 0;
            StateHasChanged();
        }
        public override async Task FreshenData()
        {
            if (Measurement is not null)
                working = Measurement;

            if (Measurement is null && measurementId > 0)
                working = await app.dbContext.Measurements.FindAsync(measurementId);

            if(working is null)
            {
                var measure = await app.dbContext.Measures.FindAsync(measureId);
                working = new MeasurementModel()
                {
                    RecipientId = recipientId,
                    RecipientTypeId = recipientTypeId,
                    RecipientType = recipientType,
                    MeasureId = measureId,
                    MeasurementUnitId = measure.UnitId,
                    DatePerformed = DateTime.Now,
                };
            }
            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (working.Id <= 0) ?
                     await app.api.ProcessCommand<MeasurementModel, CreateMeasurement>("api/CreateMeasurement", new CreateMeasurement { Measurement = working }) :
                     await app.api.ProcessCommand<MeasurementModel, UpdateMeasurement>("api/UpdateMeasurement", new UpdateMeasurement { Measurement = working });

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
            working = original.Map(working) as MeasurementModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
