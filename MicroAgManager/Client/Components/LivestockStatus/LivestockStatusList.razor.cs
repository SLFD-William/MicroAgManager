using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusList:DataComponent
    {
        protected ListTemplate<LivestockStatusModel> _listComponent;
        [CascadingParameter] LivestockTypeModel livestockType { get; set; }
        [Parameter] public IEnumerable<LivestockStatusModel>? Items { get; set; }
        
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            
            if (Items is null)
                Items = (await app.dbContext.LivestockStatuses.Where(f => f.LivestockTypeId == livestockType.Id).OrderBy(f => f.ModifiedOn).ToListAsync()).AsEnumerable();

            _listComponent.Update();
            StateHasChanged();
        }
    }
}
