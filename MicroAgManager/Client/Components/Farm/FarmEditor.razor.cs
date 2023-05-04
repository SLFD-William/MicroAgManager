using BackEnd.BusinessLogic.FarmLocation;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Farm
{
    public partial class FarmEditor : Editor<FarmLocationModel>
    {
        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        FarmLocationModel farm { get; set; }
        protected override async Task FreshenData()
        {
            if(dbContext is null) dbContext= await dbSync.GetPreparedDbContextAsync();
            var query= dbContext.Farms.AsQueryable();
            if (farmId.HasValue)
                query = query.Where(f => f.Id == farmId);
            farm = await query.OrderBy(f=>f.Id).FirstOrDefaultAsync() ?? new FarmLocationModel();
            if (string.IsNullOrEmpty(farm.Name) && !string.IsNullOrEmpty(farmName))
                farm.Name = farmName;
            editContext=new EditContext(farm);
        }
        public override async Task OnSubmit()
        {
            try
            {
                var id = farm.Id;
                if (id <= 0)
                    farm.Id= await api.ProcessCommand<FarmLocationModel, CreateFarmLocation>("api/CreateFarmLocation", new CreateFarmLocation { Farm=farm });
                else
                    farm.Id = await api.ProcessCommand<FarmLocationModel, UpdateFarmLocation>("api/UpdateFarmLocation", new UpdateFarmLocation { Farm = farm });

                if (id <= 0)
                    throw new Exception("Unable to save farm location");
                farm.Id = id;
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
