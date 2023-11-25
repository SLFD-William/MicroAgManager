using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Livestock
{
    public partial class LivestockViewer: ComponentBase
    {
        [CascadingParameter] public LivestockModel? Livestock { get; set; }
        [Parameter] public EventCallback<object> Submitted { get; set; }
        [Parameter] public EventCallback Cancelled { get; set; }

        protected override void OnInitialized()
        {
            var foo = Livestock;
        }
        private bool _editting = false;
        private void ShowEditor() => _editting = true;
        private async Task EditComplete(bool submit = false)
        {
            _editting = false;
            if (submit) await Submitted.InvokeAsync(Livestock);
            else await Cancelled.InvokeAsync();
        }

    }
}
