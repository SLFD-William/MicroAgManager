using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace MicroAgManager.Client.Components.Shared
{
    public class BaseEditor:ComponentBase
    {
        [Inject] protected ClientApplicationStateProvider _app { get; set; }
        [Parameter] required public EditContext editContext { get; set; }
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnCancel { get; set; }
        [Parameter] public bool Modal { get; set; }
        [Parameter] public bool Show { get; set; } = false;
        protected FrontEndDbContext _dbContext { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _dbContext = await _app.GetDbContextAsync();
        }
    }
}
