using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Components.Shared
{
    public abstract class DataComponent : ComponentBase, IAsyncDisposable
    {
        [Inject] protected ApplicationStateProvider app { get; set; }
        [Parameter] public bool Selectable { get; set; } = false;
        [Parameter] public bool CanDelete { get; set; } = false;
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<object> Submitted { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public bool Modal { get; set; }
        protected async override Task OnInitializedAsync()
        {
            while (app.dbSynchonizer is null)
                await Task.Delay(100);
            while (app.dbContext is null)
                await Task.Delay(100);
            app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;

            await FreshenData();
        }

        protected virtual async void DbSync_OnUpdate()
        {
            if(app.dbContext is not null)
                await FreshenData();
            StateHasChanged();
        }
      
        public virtual ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
        public abstract Task FreshenData();
        public ILogger? Log { get => app.log;}
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && app.dbContext is not null)
                await FreshenData();
        }
    }
}
