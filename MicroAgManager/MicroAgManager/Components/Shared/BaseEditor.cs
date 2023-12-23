using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace MicroAgManager.Components.Shared
{
    public class BaseEditor:ComponentBase
    {
        [Parameter] required public EditContext editContext { get; set; }
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnCancel { get; set; }
        [Parameter] public bool Modal { get; set; }
        [Parameter] public bool Show { get; set; } = false;
    }
}
