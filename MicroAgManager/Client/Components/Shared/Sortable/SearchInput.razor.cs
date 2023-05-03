using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared.Sortable
{
    public partial class SearchInput:ComponentBase
    {
        [Parameter] public EventCallback<KeyboardEventArgs> SearchKeyUpCallback { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> ShowSearchDetailCallback { get; set; }
        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public bool ShowSearchDetail { get; set; }
    }

}
