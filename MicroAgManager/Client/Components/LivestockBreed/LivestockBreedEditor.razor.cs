using BackEnd.BusinessLogic.Livestock.Breeds;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedEditor : DataComponent<LivestockBreedModel>
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedModel LivestockBreed { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        private ValidatedForm _validatedForm;
        protected new LivestockBreedModel working { get => base.working as LivestockBreedModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            working = new LivestockBreedModel() { LivestockAnimalId = livestockAnimalId.Value };

            if (LivestockBreed is not null)
                working = LivestockBreed;

            if (LivestockBreed is null && livestockBreedId.HasValue)
                working = await app.dbContext.LivestockBreeds.FindAsync(livestockBreedId);
            
            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                var id = (working?.Id <= 0) ? 
                    await app.api.ProcessCommand<LivestockBreedModel, CreateLivestockBreed>("api/CreateLivestockBreed", new CreateLivestockBreed { LivestockBreed = working }):
                    await app.api.ProcessCommand<LivestockBreedModel, UpdateLivestockBreed>("api/UpdateLivestockBreed", new UpdateLivestockBreed { LivestockBreed = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Breed");

                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }
        private void Cancel()
        {
            working = original.Map(working) as LivestockBreedModel;
            SetEditContext(working);
            Task.Run(Cancelled.InvokeAsync);
        }
    }
}
