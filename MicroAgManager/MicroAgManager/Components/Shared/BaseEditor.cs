using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Domain.Entity;
using Domain.Models;
using System.Net.NetworkInformation;
using MicroAgManager.Services;

namespace MicroAgManager.Components.Shared
{
    public abstract class BaseEditor:ComponentBase,IDisposable
    {
        [CascadingParameter] protected ApplicationState appState { get; set; }
        [Parameter] required public EditContext editContext { get; set; }
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnCancel { get; set; }
        [Parameter] public bool Modal { get; set; }
        [Parameter] public bool Show { get; set; } = false;
        protected override void OnInitialized()
        {
            appState.OnDbUpdate += Refresh;
        }
        private void Refresh() => StateHasChanged();
        public void Dispose()
        {
            appState.OnDbUpdate -= Refresh;
        }
    }
}
