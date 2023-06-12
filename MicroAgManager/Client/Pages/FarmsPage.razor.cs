using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Pages
{
    public partial class FarmsPage: DataComponent
    {
        public bool multipleFarms() => app.dbContext?.Farms.Count() > 1;
        private FarmLocationModel selectedFarm { get; set; }
        public override async Task FreshenData()
        {
            var query = app.dbContext.Farms.AsQueryable();
            if (selectedFarm is null)
                selectedFarm=await query.FirstOrDefaultAsync() ?? new FarmLocationModel();
            else
                selectedFarm = await query.SingleAsync(x=>x.Id==selectedFarm.Id);
        }
    }
}
