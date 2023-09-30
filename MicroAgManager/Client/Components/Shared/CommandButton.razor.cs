using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.Shared
{
    public partial class CommandButton : ComponentBase
    {
        [Inject] protected ApplicationStateProvider app { get; set; }
        [Parameter] public RenderFragment DataContent { get; set; }
        [Parameter] public RenderFragment SecondaryContent { get; set; }
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public bool showUpdateButton { get; set; }
        [Parameter] public bool showCancelButton { get; set; } = true;
        [Parameter] public EventCallback FreshenData { get; set; }
        [Parameter] public EventCallback Cancel { get; set; }

        protected virtual void DbSync_OnUpdate() => FreshenData.InvokeAsync();
        protected override void OnInitialized() => app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
        public virtual ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
        [Parameter] public required string Command { get; set; }
        private bool _modalOpen = false;
        private void ShowModal()
        {
            _modalOpen=true;
            StateHasChanged();
        }

        public void HideModal()
        {
            _modalOpen = false;
            StateHasChanged();
        }
    }
}
