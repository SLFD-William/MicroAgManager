using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MicroAgManager.Components.Shared
{
    public partial class ValidatedForm
    {
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnCancel { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public RenderFragment SecondaryContent { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        
        [Parameter] public string CssClass { get; set; }
        [Parameter] public bool createOnly { get; set; }
        [Parameter] public bool Modal { get; set; } = false;
        [Parameter] public bool Show { get; set; } = false;

    }
}
