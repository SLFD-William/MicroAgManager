using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared.Sortable
{
    public partial class ListViewSelector:ComponentBase
    {
        [Parameter] public ListPresentationTypesEnum ListType { get; set; }
        [Parameter] public List<ListPresentationTypesEnum> AvailableListTypes { get; set; }
        [Parameter] public bool ShowViewSelector { get; set; }
    }
}
