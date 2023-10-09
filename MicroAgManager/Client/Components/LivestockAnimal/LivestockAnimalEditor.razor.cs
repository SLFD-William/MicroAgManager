using BackEnd.BusinessLogic.Livestock.Animals;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockAnimal
{
    public partial class LivestockAnimalEditor : DataComponent<LivestockAnimalModel>
    {
        [CascadingParameter] public LivestockAnimalModel LivestockAnimal { get; set; }
        [Parameter] public long? livestockAnimalId { get; set; }
        private ValidatedForm _validatedForm;
        protected new LivestockAnimalModel working { get => base.working as LivestockAnimalModel; set { base.working = value; } }
        protected LivestockAnimalSubTabs _tabControl;

        public string Name { get => working.Name;
             set
                { if(!CheckNameExists(value).Result) working.Name = value; } }
       
        private async Task<bool> CheckNameExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (!app.dbContext.LivestockAnimals.Any(l => l.Name == working.Name && l.Id != working.Id)) return false;
            var check= await app.dbContext.LivestockAnimals.FirstOrDefaultAsync(l => l.Name == name);
            if (check is null) return false;
            working = check;
            await Submitted.InvokeAsync(working);
            editContext.MarkAsUnmodified();
            StateHasChanged();
            return true;
        }
        public override async Task FreshenData() 
        {
            if (LivestockAnimal is not null)
            {
                working = LivestockAnimal;
                editContext = new EditContext(working);
                StateHasChanged();
                return;
            }
            if (LivestockAnimal is not null)
                livestockAnimalId = LivestockAnimal.Id;

            working = new LivestockAnimalModel();

            var query = app.dbContext.LivestockAnimals.AsQueryable();
            if (livestockAnimalId.HasValue && livestockAnimalId > 0)
                query = query.Where(f => f.Id == livestockAnimalId);

            working = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockAnimalModel();

            editContext = new EditContext(working);
        }
        public  async Task OnSubmit()
        {
            try
            {
                var id = working.Id;
                if (working.Id <= 0)
                    working.Id = await app.api.ProcessCommand<LivestockAnimalModel, CreateLivestockAnimal>("api/CreateLivestockAnimal", new CreateLivestockAnimal { LivestockAnimal = working });
                else
                    working.Id = await app.api.ProcessCommand<LivestockAnimalModel, UpdateLivestockAnimal>("api/UpdateLivestockAnimal", new UpdateLivestockAnimal { LivestockAnimal = working });

                if (working.Id <= 0)
                    throw new Exception("Failed to save livestock type");

                working.Id = id;
                original = working.Clone() as LivestockAnimalModel;
                editContext = new EditContext(working);
                await Submitted.InvokeAsync(working);
                StateHasChanged();
            }
            catch (Exception ex)
            { }
        }
        private async void Cancel()
        {
            editContext = new EditContext(working);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
    }
}
