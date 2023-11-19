using BackEnd.BusinessLogic.Livestock;
using Domain.Constants;
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

        private string FemaleName { get => string.Empty; set => FemaleSelected(value); }
        void FemaleSelected(string value)
        {
            var fem = app.dbContext.Livestocks.FirstOrDefault(f => f.Name == value && f.Gender==GenderConstants.Female);
            if (fem !=null)
                serviceLivestock.DamIds.Add(fem.Id);
            editContext = new EditContext(serviceLivestock);
            StateHasChanged();
        }
        private void RemoveFemale(long id)
        {
            serviceLivestock.DamIds.Remove(id);
            editContext = new EditContext(serviceLivestock);
            StateHasChanged();
        }
        private string StudName { get
            {
               return app.dbContext.Livestocks.Find(serviceLivestock.StudId)?.Name ?? string.Empty;
            }
            set {
                    var stud= app.dbContext.Livestocks.FirstOrDefault(f => f.Name == value);
                    serviceLivestock.StudId = stud?.Id ?? 0;
                 } 
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
