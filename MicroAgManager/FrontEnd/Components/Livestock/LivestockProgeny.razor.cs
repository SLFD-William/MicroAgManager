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
        [Inject] public NavigationManager nm { get; set; }
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public long? livestockId { get; set; }

        private LivestockModel livestock { get; set; }
        private List<LivestockModel> mates { get; set; } = new();
        private List<LivestockModel> offspring { get; set; } = new();
        private List<LivestockModel> selectedMates { get; set; } = new();
        private Dictionary<long,string> MateColors=new();

        public void ToggleMate(LivestockModel mate)
        {
            if (selectedMates.Any(m => m.Id == mate.Id))
            {
                var index = selectedMates.FindIndex(m => m.Id == mate.Id);
                selectedMates.RemoveAt(index);
            }
            else
                selectedMates.Add(mate);
            StateHasChanged();
        }
        private string SelectedMateColor(LivestockModel mate)
        {
            if (!selectedMates.Any(m=>m.Id==mate.Id)) return string.Empty;
            return MateColors.ContainsKey(mate.Id) ? MateColors[mate.Id] : string.Empty;
        }
        private bool ShowUnkownButton()=> livestock.Gender == GenderConstants.Male ?
                offspring.Any(o => o.MotherId == null) :
                offspring.Any(o => o.FatherId == null);
        private string ChildStyle(LivestockModel child)
        {
            if (!selectedMates.Any()) return string.Empty;
            
            var mateId=livestock.Gender == GenderConstants.Male ? child.MotherId ?? 0 : child.FatherId ?? 0 ;
            var thisMate = selectedMates.Find(m=>m.Id==mateId);
            return thisMate is null? "display:none;" : SelectedMateColor(thisMate);
        }

        public override async Task FreshenData()
        {
            if (Livestock is not null)
                livestock = Livestock;
            if (Livestock is null && livestockId.HasValue)
                livestock = await app.dbContext.Livestocks
                    .Include(p => p.Status)
                    .Include(p => p.Breed)
                    .ThenInclude(p => p.Animal)
                    .FirstOrDefaultAsync(i => i.Id == livestockId);

            
            offspring = livestock.Gender == GenderConstants.Male ?
                await app.dbContext.Livestocks.Where(x => x.FatherId == livestock.Id).ToListAsync() :
                await app.dbContext.Livestocks.Where(x => x.MotherId == livestock.Id).ToListAsync();

          
            var mateIds = livestock.Gender == GenderConstants.Male ?
                offspring.Where(o =>o.FatherId == livestock.Id && o.MotherId.HasValue).Select(o=>o.MotherId).Distinct().ToList() :
                offspring.Where(o =>o.MotherId == livestock.Id && o.FatherId.HasValue).Select(o => o.FatherId).Distinct().ToList();

            mates = await app.dbContext.Livestocks.Where(x => mateIds.Contains(x.Id)).OrderBy(x=>x.Id).ToListAsync();
            if (ShowUnkownButton())
            { 
                var unknownMate = new LivestockModel
                {
                    Id = 0,
                    Name = "Unknown",
                    Gender = livestock.Gender == GenderConstants.Male ? GenderConstants.Female : GenderConstants.Male
                };
                mates.Add(unknownMate);
            }
            foreach (var mate in mates)
            {
                var index = mates.FindIndex(m => m.Id == mate.Id);
                var bgColor= Color.FromName(((KnownColor)index + 28).GetEnumDescription());
                var foreColor = PerceivedBrightness(bgColor) < 131 ? "white" : "black";
                var bgHexString = $"#{bgColor.R:X2}{bgColor.G:X2}{bgColor.B:X2}";
                MateColors.TryAdd(mate.Id, $"background-color:{bgHexString}; color:{foreColor};");
            }

            if (!selectedMates.Any())
                selectedMates.AddRange(mates);
            StateHasChanged();
        }
        private int PerceivedBrightness(Color c)
        {
            return (int)Math.Sqrt(c.R * c.R * .299 +
                                  c.G * c.G * .587 +
                                  c.B * c.B * .114);
        }
    }
}

