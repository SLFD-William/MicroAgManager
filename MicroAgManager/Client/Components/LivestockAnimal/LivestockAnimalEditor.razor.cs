using BackEnd.BusinessLogic.Livestock.Animals;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalEditor : Editor<LivestockAnimalModel>
    {
        [CascadingParameter] public LivestockAnimalModel livestockAnimal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
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
            if (_submitting) return;
           
            var query = app.dbContext.LivestockAnimals.AsQueryable();
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.Id == livestockAnimalId);
            livestockAnimal = new LivestockAnimalModel();
            if (!createOnly) 
                livestockAnimal = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockAnimalModel();

            editContext = new EditContext(livestockAnimal);
        }
        public override async Task OnSubmit()
        {
            try
            {
                _submitting = true;
                var id = livestockAnimal.Id;
                if (id <= 0)
                    id = await app.api.ProcessCommand<LivestockAnimalModel, CreateLivestockAnimal>("api/CreateLivestockAnimal", new CreateLivestockAnimal { LivestockAnimal = livestockAnimal });
                else
                    id = await app.api.ProcessCommand<LivestockAnimalModel, UpdateLivestockAnimal>("api/UpdateLivestockAnimal", new UpdateLivestockAnimal { LivestockAnimal = livestockAnimal });

                if (id <= 0)
                    throw new Exception("Failed to save livestock type");

                livestockAnimal.Id = id;
                editContext = new EditContext(livestockAnimal);
                await Submitted.InvokeAsync(livestockAnimal);
                _submitting = false;
                if (createOnly)
                {
                    livestockAnimal = new LivestockAnimalModel();
                    editContext = new EditContext(livestockAnimal);
                    editContext.MarkAsUnmodified();
                    await Submitted.InvokeAsync(livestockAnimal);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            { }
            finally { _submitting = false; }
                            
        }
        private async void Cancel()
        {
            editContext = new EditContext(livestockAnimal);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
    }
}
