using BackEnd.BusinessLogic.FarmLocation;
using Domain.Models;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Farm
{
    public partial class FarmEditor
    {
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        [Inject] IFrontEndApiServices api { get; set; }
        FarmLocationModel farm { get; set; }
        private EditContext? editContext;
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            var query= dbContext.Farms.AsQueryable();
            if (farmId.HasValue)
                query = query.Where(f => f.Id == farmId);
            farm = await query.FirstOrDefaultAsync() ?? new FarmLocationModel();
            if (string.IsNullOrEmpty(farm.Name) && !string.IsNullOrEmpty(farmName))
                farm.Name = farmName;
            editContext=new EditContext(farm);
        }
        async Task OnSubmit()
        {
            try
            {
                var state= farm.Id <= 0 ? EntityState.Added: EntityState.Modified;

                if (state== EntityState.Added)
                    farm.Id= await api.ProcessCommand<FarmLocationModel, CreateFarmLocation>("api/CreateFarmLocation", new CreateFarmLocation { Farm=farm });
                else
                    farm.Id = await api.ProcessCommand<FarmLocationModel, UpdateFarmLocation>("api/UpdateFarmLocation", new UpdateFarmLocation { Farm = farm });

                dbContext.Attach(farm);
                dbContext.Entry(farm).State = state;
                await dbContext.SaveChangesAsync();
                editContext = new EditContext(farm);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
