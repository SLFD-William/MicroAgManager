using Domain.Entity;
using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Components.Farm
{
    public partial class FarmList:DataComponent
    {
        public TableTemplate<FarmLocationModel> _listComponent;
        [Parameter] public IEnumerable<FarmLocationModel>? Items { get; set; }
        [Parameter] public bool Selectable { get; set; } = false;
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<FarmLocationModel>? FarmSelected { get; set; }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                FarmSelected?.Invoke(_listComponent.SelectedItems.First());
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;

            if (Items is null)
                Items = app.dbContext.Farms.OrderBy(f => f.ModifiedOn).AsEnumerable();

            StateHasChanged();
            _listComponent.Update();
        }

    }
}
