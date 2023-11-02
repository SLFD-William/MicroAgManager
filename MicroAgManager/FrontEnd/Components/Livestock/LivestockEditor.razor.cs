using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using BackEnd.BusinessLogic.Livestock;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.LivestockAnimal;
using Domain.Constants;
using FrontEnd.Components.LivestockStatus;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockEditor : DataComponent<LivestockModel>
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary LivestockBreed { get; set; }
        [CascadingParameter] public LivestockModel Livestock { get; set; }
        
        [Parameter] public long? livestockId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        private ValidatedForm _validatedForm;
        private LivestockStatusEditor _livestockStatusEditor;
        protected new LivestockModel working { get => base.working as LivestockModel; set { base.working = value; } }
        public long StatusId
        { get => working.StatusId ?? 0;
            set {
                if (value != working.StatusId)
                { 
                    working.StatusId = value;
                    var stat= app.dbContext.LivestockStatuses.Find(value);
                    if (stat.InMilk!= LivestockStatusModeConstants.Unchanged) working.InMilk = bool.Parse(stat.InMilk);
                    if (stat.BeingManaged != LivestockStatusModeConstants.Unchanged) working.BeingManaged = bool.Parse(stat.BeingManaged);
                    if (stat.BottleFed != LivestockStatusModeConstants.Unchanged) working.BottleFed = bool.Parse(stat.BottleFed);
                    if (stat.ForSale != LivestockStatusModeConstants.Unchanged) working.ForSale = bool.Parse(stat.ForSale);
                    if (stat.Sterile != LivestockStatusModeConstants.Unchanged) working.Sterile = bool.Parse(stat.Sterile);
                    StateHasChanged();
                }
            }
        }
        public override async Task FreshenData()
        {
            working= new LivestockModel();

            if (Livestock is not null)
                working = Livestock;
            
            if (LivestockBreed is null && livestockBreedId.HasValue)
                LivestockBreed = new LivestockBreedSummary(await app.dbContext.LivestockBreeds.FindAsync(livestockBreedId.Value),app.dbContext);

            livestockBreedId = LivestockBreed?.Id;

            if (LivestockAnimal is null)
                LivestockAnimal = new LivestockAnimalSummary( await app.dbContext.LivestockAnimals.FindAsync(LivestockBreed.LivestockAnimalId),app.dbContext);

            if (Livestock is null && livestockId.HasValue)
                working = await app.dbContext.Livestocks.FindAsync(livestockId);

            SetEditContext(working);
        }
        public async Task OnSubmit()
        {
            try
            {

                var id = (working?.Id <= 0) ?
                    await app.api.ProcessCommand<LivestockModel, CreateLivestock>("api/CreateLivestock", new CreateLivestock { Livestock = working }):
                    await app.api.ProcessCommand<LivestockModel, UpdateLivestock>("api/UpdateLivestock", new UpdateLivestock { Livestock = working });

                if (id <= 0)
                    throw new Exception("Failed to save working Breed");
                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
                
            }
            catch (Exception ex)
            {

            }
        }
        private bool showStatusModal = false;
        private void ShowStatusEditor()
        {
            showStatusModal = true;
            StateHasChanged();
        }
        private void StatusCanceled()
        {
            working.StatusId = ((LivestockModel)original).StatusId;
            showStatusModal = false;
            SetEditContext(working);
        }
        private void StatusCreated(object e)
        {
            var status = e as LivestockStatusModel;
            showStatusModal = false;
            working.StatusId = status?.Id;
            SetEditContext(working);
        }
        private void Cancel()
        {
            working = original.Map(working) as LivestockModel;
            SetEditContext(working);
            Task.Run(Cancelled.InvokeAsync);
        }
    }
}