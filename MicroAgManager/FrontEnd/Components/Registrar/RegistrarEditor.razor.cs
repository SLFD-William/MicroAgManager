using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Registrar;

namespace FrontEnd.Components.Registrar
{
    public partial class RegistrarEditor : DataComponent<RegistrarModel>
    {
        [CascadingParameter] public RegistrarModel Registrar { get; set; }
        [Parameter] public long? registrarId { get; set; }
        
        private ValidatedForm _validatedForm;
        public override async Task FreshenData()
        {
            working = new RegistrarModel();
            if (Registrar is not null)
                working    = Registrar;

            if (Registrar is null && registrarId.HasValue)
                working = await app.dbContext.Registrars.FindAsync(registrarId);

            SetEditContext((RegistrarModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (((RegistrarModel)working).Id <= 0) ?
                    await app.api.ProcessCommand<RegistrarModel, CreateRegistrar>("api/CreateRegistrar", new CreateRegistrar { Registrar = (RegistrarModel)working }) :
                    await app.api.ProcessCommand<RegistrarModel, UpdateRegistrar>("api/UpdateRegistrar", new UpdateRegistrar { Registrar = (RegistrarModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save Registrar");
                ((RegistrarModel)working).Id = id;
                SetEditContext((RegistrarModel)working);
                await Submitted.InvokeAsync((RegistrarModel)working);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Cancel()
        {
            working =original.Map((RegistrarModel)working);
            SetEditContext((RegistrarModel)working);
            await Cancelled.InvokeAsync((RegistrarModel)working);
            
        }
    }
}
