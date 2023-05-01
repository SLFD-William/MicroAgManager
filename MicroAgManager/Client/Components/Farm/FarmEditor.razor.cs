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
        [CascadingParameter] IFrontEndApiServices api { get; set; }

        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        
        [Parameter]public EventCallback<FarmLocationModel> Submitted { get; set; }
        FarmLocationModel farm { get; set; }
        public EditContext editContext { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            var query= dbContext.Farms.AsQueryable();
            if (farmId.HasValue)
                query = query.Where(f => f.Id == farmId);
            farm = await query.OrderBy(f=>f.Id).FirstOrDefaultAsync() ?? new FarmLocationModel();
            if (string.IsNullOrEmpty(farm.Name) && !string.IsNullOrEmpty(farmName))
                farm.Name = farmName;
            editContext=new EditContext(farm);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (farm.Id <= 0)
                    farm.Id= await api.ProcessCommand<FarmLocationModel, CreateFarmLocation>("api/CreateFarmLocation", new CreateFarmLocation { Farm=farm });
                else
                    farm.Id = await api.ProcessCommand<FarmLocationModel, UpdateFarmLocation>("api/UpdateFarmLocation", new UpdateFarmLocation { Farm = farm });

                if (farm.Id <= 0)
                    throw new Exception("Unable to save farm location");

                editContext = new EditContext(farm);
                await Submitted.InvokeAsync(farm);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
