using Domain.Constants;
using Domain.Extensions;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockProgeny : DataComponent<LivestockModel>
    {
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public long? livestockId { get; set; }

        private static LivestockAnimalModel animal { get; set; }
        private static LivestockBreedModel breed { get; set; }
        private LivestockModel livestock { get; set; }
        private List<LivestockModel> mates { get; set; } = new();
        private List<LivestockModel> offspring { get; set; } = new();
        private List<LivestockModel> selectedMates { get; set; } = new();
        public void ToggleMate(LivestockModel mate)
        { 
            if (selectedMates.Contains(mate))
                selectedMates.Remove(mate);
            else
                selectedMates.Add(mate);
            StateHasChanged();
        }
        private bool ShowUnkownButton()=> livestock.Gender == GenderConstants.Male ?
                offspring.Any(o => o.MotherId == null) :
                offspring.Any(o => o.FatherId == null);
        private string SelectedMateColor(LivestockModel mate)
        {
            if(!selectedMates.Contains(mate)) return string.Empty;
            var index= mates.IndexOf(mate);
            return index < 0 ? string.Empty :((KnownColor)index + 28).GetEnumDescription();
        }
        private string ChildStyle(LivestockModel child)
        {
            if (!selectedMates.Any()) return string.Empty;
            if (ShowUnkownButton() && selectedMates.Any(m => m.Id < 1))
                return livestock.Gender == GenderConstants.Male ?
                    (!child.MotherId.HasValue ? $"background-color:{SelectedMateColor(new())}" : string.Empty):
                    (!child.FatherId.HasValue ? $"background-color:{SelectedMateColor(new())}" : string.Empty);
            
            var thisMate= livestock.Gender == GenderConstants.Male ?
                selectedMates.FirstOrDefault(m => m.Id == child.MotherId) :
                selectedMates.FirstOrDefault(m => m.Id == child.FatherId);

            return thisMate is null? "display:none;" : $"background-color:{SelectedMateColor(thisMate)}";
        }
        public override async Task FreshenData()
        {
            if (Livestock is not null)
                livestock = Livestock;
            if (Livestock is null && livestockId.HasValue)
                livestock = await app.dbContext.Livestocks.FindAsync(livestockId);

            if(breed is null)
                breed = await app.dbContext.LivestockBreeds.FindAsync(livestock?.LivestockBreedId);
            if (animal is null)
                animal = await app.dbContext.LivestockAnimals.FindAsync(breed?.LivestockAnimalId);

            offspring = livestock.Gender == GenderConstants.Male ?
                await app.dbContext.Livestocks.Where(x => x.FatherId == livestock.Id).ToListAsync() :
                await app.dbContext.Livestocks.Where(x => x.MotherId == livestock.Id).ToListAsync();

            var mateIds = livestock.Gender == GenderConstants.Male ?
                offspring.Where(o => o.FatherId == livestock.Id).Select(o => o.MotherId).Distinct().ToList() :
                offspring.Where(o => o.MotherId == livestock.Id).Select(o => o.FatherId).Distinct().ToList();

            mates = await app.dbContext.Livestocks.Where(x => mateIds.Contains(x.Id)).OrderBy(x=>x.Id).ToListAsync();
            if(!selectedMates.Any())
                selectedMates.AddRange(mates);

            StateHasChanged();
        }
    }
}

