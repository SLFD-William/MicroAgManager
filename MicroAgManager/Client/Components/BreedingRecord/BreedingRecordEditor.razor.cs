﻿using BackEnd.BusinessLogic.BreedingRecord;
using Domain.Models;
using FrontEnd.Components.LivestockAnimal;
using FrontEnd.Components.LivestockBreed;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.BreedingRecord
{
    public partial class BreedingRecordEditor : DataComponent<BreedingRecordModel>
    {
        [CascadingParameter] public LivestockAnimalSummary LivestockAnimal { get; set; }
        [CascadingParameter] public LivestockBreedSummary Breed { get; set; }
        [CascadingParameter] public BreedingRecordModel BreedingRecord { get; set; }
        

        private ValidatedForm _validatedForm;
        [Parameter] public long? breedingRecordId { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        
        protected new BreedingRecordModel working { get => base.working as BreedingRecordModel; set { base.working = value; } }

        public override async Task FreshenData()
        {
            working = new BreedingRecordModel() { FemaleId = 0 };
            if (Breed is not null)
                livestockBreedId = Breed.Id;

            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            
            if (BreedingRecord is not null)
                working = BreedingRecord.Clone() as BreedingRecordModel;
            if (BreedingRecord is null && breedingRecordId > 0)
                working = await app.dbContext.BreedingRecords.FirstOrDefaultAsync(b=>b.Id==breedingRecordId);

            if(working?.FemaleId >0 )
                livestockBreedId=(await app.dbContext.Livestocks.FindAsync(working.FemaleId))?.LivestockBreedId;

            if (livestockBreedId.HasValue && (Breed is null || Breed.Id!=livestockBreedId))
                Breed = new LivestockBreedSummary(await app.dbContext.LivestockBreeds.FindAsync(livestockBreedId), app.dbContext);

            livestockAnimalId=Breed?.LivestockAnimalId;

            if (livestockAnimalId.HasValue && (LivestockAnimal is null || LivestockAnimal.Id!=livestockAnimalId))
                LivestockAnimal = new LivestockAnimalSummary(await app.dbContext.LivestockAnimals.FindAsync(livestockAnimalId), app.dbContext);
            
            BreedingRecord=working.Clone() as BreedingRecordModel;
            SetEditContext(working);
        }
        private async Task Cancel()
        {
            working =original.Clone() as BreedingRecordModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);
            BreedingRecord = working.Clone() as BreedingRecordModel;
            StateHasChanged();
        }
        public async Task OnSubmit()
        {
            try
            {
                var id = (working.Id <= 0) ?
                    await app.api.ProcessCommand<BreedingRecordModel, CreateBreedingRecord>("api/CreateBreedingRecord", new CreateBreedingRecord { BreedingRecord = working }) :
                    await app.api.ProcessCommand<BreedingRecordModel, UpdateBreedingRecord>("api/UpdateBreedingRecord", new UpdateBreedingRecord { BreedingRecord = working });

                if (id <= 0)
                    throw new Exception("Unable to save farm location");
                working.Id = id;
                original = working.Clone() as BreedingRecordModel;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
                BreedingRecord = working.Clone() as BreedingRecordModel;
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }


    }
}
