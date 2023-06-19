﻿using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Components.Shared
{
    public abstract class DataComponent : ComponentBase, IAsyncDisposable
    {
        [Inject] protected ApplicationStateProvider app { get; set; }
        protected async override Task OnInitializedAsync()
        {
            while (app.dbSynchonizer is null)
                await Task.Delay(100);
            while (app.dbContext is null)
                await Task.Delay(100);
            app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
        }

        protected virtual void DbSync_OnUpdate() => StateHasChanged();
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
            {
                //Log.LogWarning("First Render");
                await FreshenData();
            }
               
        }
    }
}