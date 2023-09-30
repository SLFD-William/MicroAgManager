using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared
{
    public partial class CommandButton<TValue> : ComponentBase where TValue:class
    {

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public required Action OpenModal { get; set; }
        [Parameter] public required string Command { get; set; }
        [Parameter] public required TValue Model { get; set; }
        [Parameter] public EventCallback<TValue> ModalClosed { get; set; }
        [Parameter] public EventCallback<TValue> ModalOpened { get; set; }
        private bool _modalOpen = false;
        private async Task Clicked()
        {
            _modalOpen=!_modalOpen;
            if (_modalOpen)
            {
                OpenModal?.Invoke();
                await ModalOpened.InvokeAsync(Model);
            }
            if (!_modalOpen)
                await ModalClosed.InvokeAsync(Model);

            StateHasChanged();
        }
        

    }
}
