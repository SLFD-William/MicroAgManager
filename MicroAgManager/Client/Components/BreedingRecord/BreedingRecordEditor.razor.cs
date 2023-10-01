using BackEnd.BusinessLogic.BreedingRecord;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.BreedingRecord
{
    public partial class BreedingRecordEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary Breed { get; set; }
        [CascadingParameter] public BreedingRecordModel BreedingRecord { get; set; }
        [Parameter] public EventCallback<BreedingRecordModel> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }
        private ValidatedForm _validatedForm;
        [Parameter] public bool Modal { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public long? breedingRecordId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        BreedingRecordModel breedingRecord { get; set; }
        protected override async Task OnInitializedAsync() => await FreshenData();
        public override async Task FreshenData()
        {
            if (Breed is not null)
                livestockBreedId = Breed.Id;

            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            breedingRecord = new BreedingRecordModel() { FemaleId =0};
            if (BreedingRecord is not null)
                breedingRecord = BreedingRecord;
            if (BreedingRecord is null && breedingRecordId > 0)
                breedingRecord = await app.dbContext.BreedingRecords.FindAsync(breedingRecordId);

            if(breedingRecord?.FemaleId >0 )
                livestockBreedId=(await app.dbContext.Livestocks.FindAsync(breedingRecord.FemaleId))?.LivestockBreedId;

            if (livestockBreedId.HasValue && (Breed is null || Breed.Id!=livestockBreedId))
                Breed = new LivestockBreedSummary(await app.dbContext.LivestockBreeds.FindAsync(livestockBreedId), app.dbContext);

            livestockAnimalId=Breed?.LivestockAnimalId;

            if (livestockAnimalId.HasValue && (LivestockAnimal is null || LivestockAnimal.Id!=livestockAnimalId))
                LivestockAnimal = new LivestockAnimalSummary(await app.dbContext.LivestockAnimals.FindAsync(livestockAnimalId), app.dbContext);
            
            BreedingRecord=breedingRecord;
            editContext = new EditContext(breedingRecord);
        }
        private async Task Cancel()
        {
            editContext = new EditContext(breedingRecord);
            _validatedForm.HideModal();
            await Cancelled.InvokeAsync(breedingRecord);
            StateHasChanged();
        }
        public async Task OnSubmit()
        {
            try
            {
                var id = (breedingRecord.Id <= 0) ?
                    await app.api.ProcessCommand<BreedingRecordModel, CreateBreedingRecord>("api/CreateBreedingRecord", new CreateBreedingRecord { BreedingRecord = breedingRecord }) :
                    await app.api.ProcessCommand<BreedingRecordModel, UpdateBreedingRecord>("api/UpdateBreedingRecord", new UpdateBreedingRecord { BreedingRecord = breedingRecord });

                if (id <= 0)
                    throw new Exception("Unable to save farm location");
                breedingRecord.Id = id;
                editContext = new EditContext(breedingRecord);
                await Submitted.InvokeAsync(breedingRecord);
                _validatedForm.HideModal();
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }


    }
}
