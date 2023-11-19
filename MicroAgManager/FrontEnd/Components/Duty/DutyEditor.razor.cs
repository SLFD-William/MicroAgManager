using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Duty;
using Domain.Constants;
using FrontEnd.Components.Registrar;
using Domain.Abstracts;
using FrontEnd.Components.Measure;
using FrontEnd.Components.Treatment;

namespace FrontEnd.Components.Duty
{
    public partial class DutyEditor : DataComponent<DutyModel>
    {
        [CascadingParameter] public DutyModel Duty { get; set; }
        [Parameter] public long? dutyId { get; set; }
        
        private ValidatedForm _validatedForm;
        private RegistrarEditor _registrarEditor;
        private MeasureEditor _measureEditor;
        private TreatmentEditor _treatmentEditor;
        private string Command { get=>((DutyModel)working).Command;
            set {
                ((DutyModel)working).Command = value;
                if (((DutyModel)working).Command == DutyCommandConstants.Complete)
                {
                    ((DutyModel)working).RecipientType = nameof(RecipientTypeConstants.None);
                    ((DutyModel)working).RecipientTypeId = 0;
                    ((DutyModel)working).Relationship = nameof(DutyRelationshipConstants.Self);
                }
            } }
        
        public override async Task FreshenData()
        {
            if (Duty is not null)
                working = Duty;

            if (Duty is null && dutyId.HasValue)
                working = await app.dbContext.Duties.FindAsync(dutyId);

            if (working is null)
                working = new DutyModel();

            SetEditContext((DutyModel)working);
        }
        public async Task OnSubmit()
        {
                long id = (working.Id <= 0) ?
                    await app.api.ProcessCommand<DutyModel, CreateDuty>("api/CreateDuty", new CreateDuty { Duty = (DutyModel)working }) :
                    await app.api.ProcessCommand<DutyModel, UpdateDuty>("api/UpdateDuty", new UpdateDuty { Duty = (DutyModel)working });

                if (id <= 0)        
                    throw new Exception("Failed to save Duty");

                working.Id = id;
                SetEditContext((DutyModel)working);
                await Submitted.InvokeAsync((DutyModel)working);
        }
        private async Task Cancel()
        {
            working =original.Map((DutyModel)working);
            SetEditContext((DutyModel)working);
            await Cancelled.InvokeAsync((DutyModel)working);
        }
        private List<KeyValuePair<long, string>> recipientTypeIds()
        {
            switch (((DutyModel)working).RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.LivestockAnimals.OrderBy(a=>a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private List<KeyValuePair<long, string>> commandIds()
        {
            switch (((DutyModel)working).Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    return app.dbContext.Registrars.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                case nameof(DutyCommandConstants.Measurement):
                    return app.dbContext.Measures.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                case nameof(DutyCommandConstants.Treatment):
                    return app.dbContext.Treatments.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private bool showCommandId()
        {
            switch (((DutyModel)working).Command)
            {
                case "":
                case null:
                case nameof(DutyCommandConstants.Birth):
                case nameof(DutyCommandConstants.Breed):
                case nameof(DutyCommandConstants.Complete):
                case nameof(DutyCommandConstants.Reap):
                case nameof(DutyCommandConstants.Service):
                    return false;
                default:
                    return true;
            }
        }
        private string commandLabel()
        {
            switch (((DutyModel)working).Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    return "Registrar";
                case nameof(DutyCommandConstants.Measurement):
                    return "Measure";
                case nameof(DutyCommandConstants.Treatment):
                    return "Treatment";
                default:
                    return string.Empty;
            }
        }


        private bool showCommandModal=false;

        private void ShowCommandEditor()
        {
            showCommandModal = true;
            StateHasChanged();
        }
        private void CommandCanceled()
        {
            ((DutyModel)working).CommandId = ((DutyModel)original).CommandId;
            showCommandModal = false;
            StateHasChanged();
        }
        private void CommandCreated(object e)
        {
            var status = e as BaseModel;
            showCommandModal = false;
            ((DutyModel)working).CommandId = status?.Id ?? 0;
            StateHasChanged();
        }
    }
}
