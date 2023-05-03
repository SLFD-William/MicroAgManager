using BackEnd.BusinessLogic.FarmLocation;
using Domain.Models;
using FrontEnd.Data;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Farm
{
    public partial class FarmEditor : IAsyncDisposable
    {
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [CascadingParameter] IFrontEndApiServices api { get; set; }

        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        
        [Parameter]public EventCallback<FarmLocationModel> Submitted { get; set; }
        FarmLocationModel farm { get; set; }
        public EditContext editContext { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            dbSync.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData()
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
        public async Task OnSubmit()
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
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
