using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockAncestry : DataComponent<LivestockModel>
    {
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public long? livestockId { get; set; }
        private LivestockAnimalModel animal { get; set; } = new LivestockAnimalModel();
        private LivestockBreedModel breed { get; set; } = new LivestockBreedModel();
        private LivestockModel livestock { get; set; } = new LivestockModel();
        private LivestockModel livestockMom { get; set; } = new LivestockModel();
        private LivestockModel livestockDad { get; set; } = new LivestockModel();
        

        private string GenderTitle() => livestock.Gender == GenderConstants.Male ? animal?.ParentMaleName : animal?.ParentFemaleName;
        public override async Task FreshenData()
        {
            if (Livestock is not null)
                livestock = Livestock;
            if (Livestock is null && livestockId.HasValue)
                livestock = await app.dbContext.Livestocks.FindAsync(livestockId);

            livestockMom= await app.dbContext.Livestocks.FindAsync(livestock?.MotherId);
            livestockDad = await app.dbContext.Livestocks.FindAsync(livestock?.FatherId);
            breed = await app.dbContext.LivestockBreeds.FindAsync(livestock?.LivestockBreedId);
            animal = await app.dbContext.LivestockAnimals.FindAsync(breed?.LivestockAnimalId);

            StateHasChanged();
        }

    }
}
