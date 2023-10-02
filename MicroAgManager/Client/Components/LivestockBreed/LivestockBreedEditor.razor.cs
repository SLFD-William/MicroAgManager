using BackEnd.BusinessLogic.Livestock.Breeds;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedModel LivestockBreed { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }

        private LivestockBreedModel livestockBreed;
        private ValidatedForm _validatedForm;
        protected override async Task OnInitializedAsync() => await FreshenData();
        public override async Task FreshenData()
        {
            if (LivestockBreed is not null)
            {
                livestockBreed = LivestockBreed;
                editContext = new EditContext(livestockBreed);
                StateHasChanged();
                return;
            }
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            livestockBreed = new LivestockBreedModel() { LivestockAnimalId = livestockAnimalId.Value };
            var query = app.dbContext.LivestockBreeds.AsQueryable();
            if (livestockBreedId.HasValue && livestockBreedId > 0)
                query = query.Where(f => f.Id == livestockBreedId);
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.LivestockAnimalId == livestockAnimalId);


            editContext = new EditContext(livestockBreed);
        }
        public async Task OnSubmit()
        {
            try
            {

                if (livestockBreed.Id <= 0)
                    livestockBreed.Id = await app.api.ProcessCommand<LivestockBreedModel, CreateLivestockBreed>("api/CreateLivestockBreed", new CreateLivestockBreed { LivestockBreed = livestockBreed });
                else
                    livestockBreed.Id = await app.api.ProcessCommand<LivestockBreedModel, UpdateLivestockBreed>("api/UpdateLivestockBreed", new UpdateLivestockBreed { LivestockBreed = livestockBreed });

                if (livestockBreed.Id <= 0)
                    throw new Exception("Failed to save livestock Breed");

                editContext = new EditContext(livestockBreed);
                await Submitted.InvokeAsync(livestockBreed);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async void Cancel()
        {
            editContext = new EditContext(livestockBreed);
            await Cancelled.InvokeAsync();
            _validatedForm.HideModal();
            StateHasChanged();
        }
    }
}
