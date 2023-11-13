using BackEnd.BusinessLogic.Livestock;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class ServiceLivestockCommand : DataComponent<LivestockBreedModel>
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary LivestockBreed { get; set; }

        private CommandButton _commandButton;
        private ServiceLivestock serviceLivestock { get; set; }=new ServiceLivestock {ServiceDate=DateTime.Today, DamIds=new() };
       
        public async Task OnSubmit()
        {
            try
            {

                await app.api.ProcessCommand<LivestockBreedModel, ServiceLivestock>("api/ServiceLivestock", serviceLivestock);
                await Submitted.InvokeAsync();
                _commandButton.HideModal();
                serviceLivestock = new ServiceLivestock { ServiceDate = DateTime.Today, DamIds = new() };
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        void FemaleSelected(ChangeEventArgs e)
        {
            serviceLivestock.DamIds.Add(long.Parse(e.Value.ToString()));
            editContext = new EditContext(serviceLivestock);
            StateHasChanged();
        }
        private async Task Cancel()
        {
            //editContext = new EditContext(livestockBreed);
            _commandButton.HideModal();
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
        public override async Task FreshenData()
        {
            editContext = new EditContext(serviceLivestock);
        }
    }
}
