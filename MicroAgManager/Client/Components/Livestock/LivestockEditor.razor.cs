using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using BackEnd.BusinessLogic.Livestock;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockAnimal;
using Domain.Constants;
using FrontEnd.Components.LivestockStatus;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary LivestockBreed { get; set; }
        [CascadingParameter] public LivestockModel Livestock { get; set; }
        
        [Parameter] public long? livestockId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        private ValidatedForm _validatedForm;
        private LivestockStatusEditor _livestockStatusEditor;
        private LivestockModel livestock;
        public long StatusId
        { get => livestock.StatusId ?? 0;
            set {
                if (value != livestock.StatusId)
                { 
                    livestock.StatusId = value;
                    var stat= app.dbContext.LivestockStatuses.Find(value);
                    if (stat.InMilk!= LivestockStatusModeConstants.Unchanged) livestock.InMilk = bool.Parse(stat.InMilk);
                    if (stat.BeingManaged != LivestockStatusModeConstants.Unchanged) livestock.BeingManaged = bool.Parse(stat.BeingManaged);
                    if (stat.BottleFed != LivestockStatusModeConstants.Unchanged) livestock.BottleFed = bool.Parse(stat.BottleFed);
                    if (stat.ForSale != LivestockStatusModeConstants.Unchanged) livestock.ForSale = bool.Parse(stat.ForSale);
                    if (stat.Sterile != LivestockStatusModeConstants.Unchanged) livestock.Sterile = bool.Parse(stat.Sterile);
                    StateHasChanged();
                }
            }
        }

        public override async Task FreshenData()
        {
            if (LivestockBreed is not null)
                livestockBreedId = LivestockBreed.Id;
            
            if (Livestock is not null && Livestock.Id>0)
            {
                livestockId = Livestock.Id;
                livestock = Livestock;
            }
            
            if (LivestockBreed is null && livestockBreedId.HasValue)
                LivestockBreed = new LivestockBreedSummary(await app.dbContext.LivestockBreeds.FindAsync(livestockBreedId.Value),app.dbContext);

            if (LivestockAnimal is null)
                LivestockAnimal = new LivestockAnimalSummary( await app.dbContext.LivestockAnimals.FindAsync(LivestockBreed.LivestockAnimalId),app.dbContext);

            if ((Livestock is null || Livestock.Id<1) && livestockId > 0)
                livestock = await app.dbContext.Livestocks.FindAsync(livestockId)  ;

            if (livestock is null)
                livestock = new LivestockModel() { LivestockBreedId = livestockBreedId.HasValue ? livestockBreedId.Value : 0 };
            Livestock = livestock;
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
        private long? originalStatusId;
        private bool showStatusModal = false;
        private void ShowStatusEditor()
        {
            originalStatusId = livestock.StatusId;
            showStatusModal = true;
            StateHasChanged();
        }
        private void StatusCanceled()
        {
            livestock.StatusId = originalStatusId;
            showStatusModal = false;
            originalStatusId = null;
            StateHasChanged();
        }
        private void StatusCreated(object e)
        {
            var status = e as LivestockStatusModel;
            showStatusModal = false;
            livestock.StatusId = status?.Id;
            StateHasChanged();
        }
        private async void Cancel()
        {
            if(originalStatusId.HasValue) livestock.StatusId = originalStatusId.Value;

            editContext = new EditContext(livestock);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
    }
}