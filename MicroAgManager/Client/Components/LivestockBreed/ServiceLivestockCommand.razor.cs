using BackEnd.BusinessLogic.Livestock;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class ServiceLivestockCommand : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary LivestockBreed { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }

        private CommandButton _commandButton;
        private ServiceLivestock serviceLivestock { get; set; }=new ServiceLivestock {ServiceDate=DateTime.Today, DamIds=new() };
        protected override async Task OnInitializedAsync() => await FreshenData();
        public async Task OnSubmit()
        {
            try
            {

                await app.api.ProcessCommand<LivestockBreedModel, ServiceLivestock>("api/ServiceLivestock", serviceLivestock);
                //if (livestockBreed.Id <= 0)
                //    livestockBreed.Id = 
                //else
                //    livestockBreed.Id = await app.api.ProcessCommand<LivestockBreedModel, UpdateLivestockBreed>("api/UpdateLivestockBreed", new UpdateLivestockBreed { LivestockBreed = livestockBreed });
                //if (livestockBreed.Id <= 0)
                //    throw new Exception("Failed to save livestock Breed");

                //editContext = new EditContext(livestockBreed);
                await Submitted.InvokeAsync();
                _commandButton.HideModal();
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
