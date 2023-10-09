using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Registrar;

namespace FrontEnd.Components.Registrar
{
    public partial class RegistrarEditor : DataComponent<RegistrarModel>
    {
        [CascadingParameter] public RegistrarModel Registrar { get; set; }
        [Parameter] public long? registrarId { get; set; }
        
        private ValidatedForm _validatedForm;
        protected new RegistrarModel working { get => base.working as RegistrarModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            working = new RegistrarModel();

            if (Registrar is not null)
                working    = Registrar;

            if (Registrar is null && registrarId.HasValue)
                working = await app.dbContext.Registrars.FindAsync(registrarId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                long id = (working.Id <= 0) ?
                    await app.api.ProcessCommand<RegistrarModel, CreateRegistrar>("api/CreateRegistrar", new CreateRegistrar { Registrar = working }) :
                    await app.api.ProcessCommand<RegistrarModel, UpdateRegistrar>("api/UpdateRegistrar", new UpdateRegistrar { Registrar = working });

                if (id <= 0)
                    throw new Exception("Failed to save Registrar");
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
            working =((RegistrarModel) original).Map(working) as RegistrarModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);
            
        }
    }
}
