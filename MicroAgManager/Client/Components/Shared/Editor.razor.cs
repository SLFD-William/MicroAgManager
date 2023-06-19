using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.Shared
{
    public abstract class Editor<T>:ComponentBase, IAsyncDisposable where T : class
    {
        [Inject]protected ApplicationStateProvider app { get; set; }
        [Parameter] public bool createOnly { get; set; }
        public EditContext editContext { get; set; }
        [Parameter] public EventCallback<T> Submitted { get; set; }
        [Parameter] public EventCallback<T> Cancelled { get; set; }
        protected bool _submitting = false;
        public abstract Task FreshenData();
        public abstract Task OnSubmit();
        protected virtual void DbSync_OnUpdate() => Task.Run(FreshenData);
        protected override void OnInitialized()=>app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
            
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender) await FreshenData();
        }


        public virtual ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
