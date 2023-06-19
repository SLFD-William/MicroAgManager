using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockBreed
{
    public partial class LivestockBreedList:DataComponent
    {
        public ListTemplate<LivestockBreedModel> _listComponent;
        [CascadingParameter] LivestockTypeModel livestockType { get; set; }
        [Parameter] public IEnumerable<LivestockBreedModel>? Items { get; set; }

        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = app.dbContext.LivestockBreeds.Where(f => f.LivestockTypeId == livestockType.Id).OrderBy(f => f.ModifiedOn).AsEnumerable();

            StateHasChanged();
            _listComponent.Update();
        }
        
    }
}
