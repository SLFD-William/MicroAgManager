using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using BackEnd.BusinessLogic.Livestock;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockAnimal;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary LivestockBreed { get; set; }
        [CascadingParameter] public LivestockModel Livestock { get; set; }
        
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<LivestockModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public long? livestockId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }

        private LivestockModel livestock;

        //protected LivestockSubTabs _tabControl;

        protected override async Task OnInitializedAsync() => await FreshenData();
        public override async Task FreshenData()
        {
            if (Livestock is not null)
            {
                livestock = Livestock;
                editContext = new EditContext(livestock);
                StateHasChanged();
                return;
            }
            if (LivestockBreed is not null)
                livestockBreedId = LivestockBreed.Id;

            if (LivestockBreed is null && livestockBreedId.HasValue)
                LivestockBreed = new LivestockBreedSummary(await app.dbContext.LivestockBreeds.FindAsync(livestockBreedId.Value),app.dbContext);

            if (LivestockAnimal is null)
                LivestockAnimal = new LivestockAnimalSummary( await app.dbContext.LivestockAnimals.FindAsync(LivestockBreed.LivestockAnimalId),app.dbContext);


            livestock = new LivestockModel() { LivestockBreedId = livestockBreedId.Value };
            var query = app.dbContext.Livestocks.AsQueryable();
            if (livestockId.HasValue && livestockId > 0)
                query = query.Where(f => f.Id == livestockId);
            if (livestockBreedId.HasValue && livestockBreedId > 0)
                query = query.Where(f => f.LivestockBreedId == livestockBreedId);


            editContext = new EditContext(livestock);
        }
        public async Task OnSubmit()
        {
            try
            {

                var state = livestock.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (livestock.Id <= 0)
                    livestock.Id = await app.api.ProcessCommand<LivestockModel, CreateLivestock>("api/CreateLivestock", new CreateLivestock { Livestock = livestock });
                else
                    livestock.Id = await app.api.ProcessCommand<LivestockModel, UpdateLivestock>("api/UpdateLivestock", new UpdateLivestock { Livestock = livestock });

                if (livestock.Id <= 0)
                    throw new Exception("Failed to save livestock Breed");

                editContext = new EditContext(livestock);
                await Submitted.InvokeAsync(livestock);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async void Cancel()
        {
            editContext = new EditContext(livestock);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
    }
}