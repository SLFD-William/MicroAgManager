using BackEnd.BusinessLogic.Livestock.Breeds;
using Domain.Entity;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedEditor : Editor<LivestockBreedModel>
    {
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public long? livestockBreedId { get; set; }
        [Parameter] public LivestockBreedModel livestockBreed { get; set; }
        public override async Task FreshenData()
        {
            if (dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            livestockBreed= new LivestockBreedModel { LivestockTypeId = livestockType.Id };
            var query = dbContext.LivestockBreeds.AsQueryable();
            if (livestockBreedId.HasValue && livestockBreedId > 0)
                query = query.Where(f => f.Id == livestockBreedId);
            if (!createOnly)
                livestockBreed = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockBreedModel { LivestockTypeId = livestockType.Id };
            
            editContext = new EditContext(livestockType);
        }
        public async override Task OnSubmit()
        {
            try
            {
                _submitting = true;
                var id = livestockBreed.Id;
                if (id <= 0)
                    id = await api.ProcessCommand<LivestockBreedModel, CreateLivestockBreed>("api/CreateLivestockBreed", new CreateLivestockBreed { LivestockBreed = livestockBreed });
                else
                    id = await api.ProcessCommand<LivestockBreedModel, UpdateLivestockBreed>("api/UpdateLivestockBreed", new UpdateLivestockBreed { LivestockBreed = livestockBreed });

                if (id <= 0)
                    throw new Exception("Failed to save livestock Breed");

                livestockBreed.Id = id;
                editContext = new EditContext(livestockBreed);
                await Submitted.InvokeAsync(livestockBreed);
                _submitting = false;
                if (createOnly)
                {
                    livestockBreed = new LivestockBreedModel { LivestockTypeId = livestockType.Id };
                    editContext = new EditContext(livestockBreed);
                    editContext.MarkAsUnmodified();
                    await Submitted.InvokeAsync(livestockBreed);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
            finally { _submitting = false; }
        }
    }
}
