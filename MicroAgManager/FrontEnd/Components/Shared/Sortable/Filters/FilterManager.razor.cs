﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Components.Shared.Sortable.Filters
{
    public partial class FilterManager<TableItem>:ComponentBase
    {
        [Parameter] public IColumn<TableItem> Column { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Inject] public ILogger<FilterManager<TableItem>> Logger { get; set; }

        private void ApplyFilter()
        {
            Column.ToggleFilter();

            if (Column.FilterControl != null)
            {
                Column.Filter = Column.FilterControl.GetFilter();
                Column.SortableDataContainer.Update();
                Column.SortableDataContainer.FirstPage();
            }
            else
            {
                Logger.LogInformation("Filter is null");
            }
        }

        private void ClearFilter()
        {
            Column.ToggleFilter();

            if (Column.Filter != null)
            {
                Column.Filter = null;
                Column.SortableDataContainer.Update();
            }
        }
    }
}
