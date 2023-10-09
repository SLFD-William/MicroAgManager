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
            if (LivestockBreed is not null)
            {
                working = LivestockBreed;
                editContext = new EditContext(working);
                StateHasChanged();
                return;
            }
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            working = new LivestockBreedModel() { LivestockAnimalId = livestockAnimalId.Value };
            var query = app.dbContext.LivestockBreeds.AsQueryable();
            if (livestockBreedId.HasValue && livestockBreedId > 0)
                query = query.Where(f => f.Id == livestockBreedId);
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.LivestockAnimalId == livestockAnimalId);


            editContext = new EditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                if (working.Id <= 0)
                    working.Id = await app.api.ProcessCommand<LivestockBreedModel, CreateLivestockBreed>("api/CreateLivestockBreed", new CreateLivestockBreed { LivestockBreed = working });
                else
                    working.Id = await app.api.ProcessCommand<LivestockBreedModel, UpdateLivestockBreed>("api/UpdateLivestockBreed", new UpdateLivestockBreed { LivestockBreed = working });

                if (working.Id <= 0)
                    throw new Exception("Failed to save livestock Breed");
                original= working.Clone() as LivestockBreedModel;
                editContext = new EditContext(working);
                await Submitted.InvokeAsync(working);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async void Cancel()
        {
            editContext = new EditContext(working);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
    }
}
