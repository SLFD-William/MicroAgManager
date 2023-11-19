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
        protected LivestockAnimalSubTabs _tabControl;

        public string Name { get => ((LivestockAnimalModel)working).Name;
             set
                { if(!CheckNameExists(value).Result) ((LivestockAnimalModel)working).Name = value; } }
       
        private async Task<bool> CheckNameExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (!app.dbContext.LivestockAnimals.Any(l => l.Name == ((LivestockAnimalModel)working).Name && l.Id != ((LivestockAnimalModel)working).Id)) return false;
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

            SetEditContext((LivestockAnimalModel)working);
        }
        public  async Task OnSubmit()
        {
            try
            {
                var id = (working?.Id <= 0) ?
                    await app.api.ProcessCommand<LivestockAnimalModel, CreateLivestockAnimal>("api/CreateLivestockAnimal", new CreateLivestockAnimal { LivestockAnimal = (LivestockAnimalModel)working }):
                    await app.api.ProcessCommand<LivestockAnimalModel, UpdateLivestockAnimal>("api/UpdateLivestockAnimal", new UpdateLivestockAnimal { LivestockAnimal = (LivestockAnimalModel)working });

                if (id <= 0)
                    throw new Exception("Failed to save livestock type");

                ((LivestockAnimalModel)working).Id = id;
                SetEditContext((LivestockAnimalModel)working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            { }
        }
        private void Cancel()
        {
            working = original.Map((LivestockAnimalModel)working);
            SetEditContext((LivestockAnimalModel)working);
            Task.Run(Cancelled.InvokeAsync);
        }
    }
}
