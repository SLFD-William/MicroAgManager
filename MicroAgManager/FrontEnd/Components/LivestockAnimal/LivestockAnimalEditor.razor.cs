using BackEnd.BusinessLogic.Livestock.Animals;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
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
            working = new LivestockAnimalModel();
            if (LivestockAnimal is not null)
                working = LivestockAnimal;
               
            if (LivestockAnimal is null && livestockAnimalId.HasValue)
                working = await app.dbContext.LivestockAnimals.FindAsync(livestockAnimalId);

            SetEditContext(working);
        }
        public  async Task OnSubmit()
        {
            try
            {
                var id = (working?.Id <= 0) ?
                    await app.api.ProcessCommand<LivestockAnimalModel, CreateLivestockAnimal>("api/CreateLivestockAnimal", new CreateLivestockAnimal { LivestockAnimal = working }):
                    await app.api.ProcessCommand<LivestockAnimalModel, UpdateLivestockAnimal>("api/UpdateLivestockAnimal", new UpdateLivestockAnimal { LivestockAnimal = working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock type");

                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            { }
        }
        private void Cancel()
        {
            working = original.Map(working) as LivestockAnimalModel;
            SetEditContext(working);
            Task.Run(Cancelled.InvokeAsync);
        }
    }
}
