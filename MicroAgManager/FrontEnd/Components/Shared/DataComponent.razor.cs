﻿using Domain.Abstracts;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Components.Shared
{
    public abstract class DataComponent<Model> : ComponentBase, IAsyncDisposable where Model : BaseModel, new()
    {
        [Inject] protected ApplicationStateProvider app { get; set; }
        [Parameter] public bool Selectable { get; set; } = false;
        [Parameter] public bool CanDelete { get; set; } = false;
        [Parameter] public EventCallback Cancelled { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public EventCallback<object> Submitted { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public bool Modal { get; set; }
        [Parameter] public bool Show { get; set; } = false;
        protected BaseModel original { get; set; }
        protected BaseModel working { get; set; }
        protected async override Task OnParametersSetAsync() 
        {
            var initialized = app.dbSynchonizer is not null;
            while (app.dbSynchonizer is null)
                await Task.Delay(1000);
            while (app.dbContext is null)
                await Task.Delay(1000);
            if (!initialized)
                app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
            if (Modal && !Show) return;

            await FreshenData();
        }
        protected void SetEditContext(Model work)
        {
            working = work;
            if (working is null)
            {
                original = null;
                return;
            }
            original = new Model();
            original = working.Map(original);
            editContext = new EditContext(working);
            StateHasChanged();

        }
        protected virtual void DbSync_OnUpdate()
        {
            if (Modal && !Show) return;
            if (app.dbContext is not null)
                Task.Run(FreshenData);
            StateHasChanged();
        }
      
        public virtual ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
        public abstract Task FreshenData();
        public ILogger? Log { get => app.log;}
        
    }
}
