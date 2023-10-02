using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Duty;
using Domain.Constants;
using FrontEnd.Components.Registrar;
using Domain.Abstracts;

namespace FrontEnd.Components.Duty
{
    public partial class DutyEditor : DataComponent
    {
        [CascadingParameter] public DutyModel Duty { get; set; }
        [Parameter] public long? dutyId { get; set; }
        private DutyModel duty { get; set; }
        private ValidatedForm _validatedForm;
        private RegistrarEditor _registrarEditor;
        private string Command { get=>duty.Command;
            set { duty.Command = value;
                if (duty.Command == DutyCommandConstants.Complete)
                {
                    duty.RecipientType = nameof(RecipientTypeConstants.None);
                    duty.RecipientTypeId = 0;
                    duty.Relationship = nameof(DutyRelationshipConstants.Self);
                }
            } }
        protected override async Task OnInitializedAsync() => await FreshenData();
        public override async Task FreshenData()
        {
            duty = new DutyModel();

            if (Duty is not null)
                duty = Duty;

            if (Duty is null && dutyId.HasValue)
                duty = await app.dbContext.Duties.FindAsync(dutyId);

            editContext = new EditContext(duty);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (duty.Id <= 0) ?
                    await app.api.ProcessCommand<DutyModel, CreateDuty>("api/CreateDuty", new CreateDuty { Duty = duty }) :
                    await app.api.ProcessCommand<DutyModel, UpdateDuty>("api/UpdateDuty", new UpdateDuty { Duty = duty });

                if (id <= 0)
                    throw new Exception("Failed to save Duty");

                duty.Id = id;

                editContext = new EditContext(duty);
                await Submitted.InvokeAsync(duty);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {
               
            }
        }
        private async Task Cancel()
        {
            if (originalCommandId.HasValue) duty.CommandId = originalCommandId.Value;
            editContext = new EditContext(duty);
            await Cancelled.InvokeAsync(duty);
            _validatedForm.HideModal();
            StateHasChanged();
        }
        private List<KeyValuePair<long, string>> recipientTypeIds()
        {
            switch (duty.RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.LivestockAnimals.OrderBy(a=>a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private List<KeyValuePair<long, string>> commandIds()
        {
            switch (duty.Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    return app.dbContext.Registrars.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        private bool showCommandId()
        {
            switch (duty.Command)
            {
                case "":
                case null:
                case nameof(DutyCommandConstants.Birth):
                case nameof(DutyCommandConstants.Breed):
                case nameof(DutyCommandConstants.Complete):
                case nameof(DutyCommandConstants.Death):
                case nameof(DutyCommandConstants.Service):
                    return false;
                default:
                    return true;
            }
        }
        private string commandLabel()
        {
            switch (duty.Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    return "Registrar";
                default:
                    return string.Empty;
            }
        }


        private bool showCommandModal=false;
        private long? originalCommandId;

        private void CloseCommandModals()
        {
            showCommandModal = false;
            _registrarEditor.HideModal();
        }
        private void ShowCommandEditor()
        {
            originalCommandId= duty.CommandId;
            showCommandModal = true;
            StateHasChanged();
        }
        private void CommandCanceled()
        {
            duty.CommandId = originalCommandId.HasValue? originalCommandId.Value:0;
            CloseCommandModals();
            
            originalCommandId = null;
            StateHasChanged();
        }
        private void CommandCreated(object e)
        {
            var status = e as BaseModel;
            CloseCommandModals();
            duty.CommandId = status?.Id ?? 0;
            StateHasChanged();
        }
    }
}
