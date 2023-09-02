﻿using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Components.Shared
{
    public partial class ValidatedForm<TValue>
    {
        [Inject] protected ApplicationStateProvider app { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
        [Parameter] public EditContext editContext { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Parameter] public EventCallback FreshenData{ get; set; }
        [Parameter] public EventCallback Cancel { get; set; }
        
        [Parameter] public bool createOnly { get; set; }


        protected virtual void DbSync_OnUpdate() => FreshenData.InvokeAsync();
        protected override void OnInitialized() => app.dbSynchonizer.OnUpdate += DbSync_OnUpdate;
        public virtual ValueTask DisposeAsync()
        {
            app.dbSynchonizer.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
