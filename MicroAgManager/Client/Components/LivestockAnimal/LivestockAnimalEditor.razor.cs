using BackEnd.BusinessLogic.Livestock.Animals;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalEditor : DataComponent
    {
        [CascadingParameter] public LivestockAnimalModel LivestockAnimal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        private ValidatedForm _validatedForm;

        protected LivestockAnimalSubTabs _tabControl;
        private LivestockAnimalModel livestockAnimal;

        public string Name { get => livestockAnimal.Name;
             set
                { if(!CheckNameExists(value).Result) livestockAnimal.Name = value; } }
       
        private async Task<bool> CheckNameExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (!app.dbContext.LivestockAnimals.Any(l => l.Name == livestockAnimal.Name && l.Id != livestockAnimal.Id)) return false;
            var check= await app.dbContext.LivestockAnimals.FirstOrDefaultAsync(l => l.Name == name);
            if (check is null) return false;
            livestockAnimal = check;
            await Submitted.InvokeAsync(livestockAnimal);
            editContext.MarkAsUnmodified();
            StateHasChanged();
            return true;
        }
        public override async Task FreshenData() 
        {
            if (LivestockAnimal is not null)
            {
                livestockAnimal = LivestockAnimal;
                editContext = new EditContext(livestockAnimal);
                StateHasChanged();
                return;
            }
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            livestockAnimal = new LivestockAnimalModel();

            var query = app.dbContext.LivestockAnimals.AsQueryable();
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.Id == livestockAnimalId);

            livestockAnimal = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockAnimalModel();

            editContext = new EditContext(livestockAnimal);
        }
        public  async Task OnSubmit()
        {
            try
            {
                var id = livestockAnimal.Id;
                if (livestockAnimal.Id <= 0)
                    livestockAnimal.Id = await app.api.ProcessCommand<LivestockAnimalModel, CreateLivestockAnimal>("api/CreateLivestockAnimal", new CreateLivestockAnimal { LivestockAnimal = livestockAnimal });
                else
                    livestockAnimal.Id = await app.api.ProcessCommand<LivestockAnimalModel, UpdateLivestockAnimal>("api/UpdateLivestockAnimal", new UpdateLivestockAnimal { LivestockAnimal = livestockAnimal });

                if (livestockAnimal.Id <= 0)
                    throw new Exception("Failed to save livestock type");

                livestockAnimal.Id = id;
                editContext = new EditContext(livestockAnimal);
                await Submitted.InvokeAsync(livestockAnimal);
                StateHasChanged();
            }
            catch (Exception ex)
            { }
        }
        private async void Cancel()
        {
            editContext = new EditContext(livestockAnimal);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
    }
}
