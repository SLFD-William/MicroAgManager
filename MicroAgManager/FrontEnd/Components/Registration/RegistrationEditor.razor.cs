using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Registration;
using Domain.Abstracts;
using FrontEnd.Components.Registrar;

namespace FrontEnd.Components.Registration
{
    public partial class RegistrationEditor : HasRecipientComponent<RegistrationModel>
    {
        [CascadingParameter] public RegistrationModel Registration { get; set; }
        [Parameter] public long? registrationId { get; set; }
        [Parameter] public required long registrarId { get; set; }

        private ValidatedForm _validatedForm;
        #region Registrar
        private bool showRegistrarModal = false;
        private RegistrarEditor _registrarEditor;
        private void ShowRegistrarEditor()
        {
            showRegistrarModal = true;
            StateHasChanged();
        }
        private void RegistrarCanceled()
        {
            ((RegistrationModel)working).RegistrarId = ((RegistrationModel)original).RegistrarId;
            showRegistrarModal = false;
            StateHasChanged();
        }
        private void RegistrarCreated(object e)
        {
            var status = e as BaseModel;
            showRegistrarModal = false;
            ((RegistrationModel)working).RegistrarId = status?.Id ?? 0;
            StateHasChanged();
        }
        #endregion
        public override async Task FreshenData()
        {
            if (Registration is not null)
                working = Registration;

            if (Registration is null && registrationId > 0)
                working = await app.dbContext.Registrations.FindAsync(registrationId);

            if (working is null && registrarId>0)
                working = new RegistrationModel()
                {
                    RecipientId = RecipientId,
                    RecipientTypeId = RecipientTypeId,
                    RecipientType = RecipientType,
                    RegistrarId = registrarId,
                    RegistrationDate = DateTime.Now,
                };
    
            SetEditContext((RegistrationModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {

                long id = (((RegistrationModel)working).Id <= 0) ?
                     await app.api.ProcessCommand<RegistrationModel, CreateRegistration>("api/CreateRegistration", new CreateRegistration { Registration = (RegistrationModel)working }) :
                     await app.api.ProcessCommand<RegistrationModel, UpdateRegistration>("api/UpdateRegistration", new UpdateRegistration { Registration = (RegistrationModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Status");

                ((RegistrationModel)working).Id = id;
                SetEditContext((RegistrationModel)working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task Cancel()
        {
            working = original.Map((RegistrationModel)working);
            SetEditContext((RegistrationModel)working);
            await Cancelled.InvokeAsync(working);

        }
    }
}
