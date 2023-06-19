﻿using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared
{
    public partial class TabPage : ComponentBase
    {
        [CascadingParameter] private TabControl Parent { get; set; }
        [Parameter] public RenderFragment ButtonContent { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent), "TabPage must exist within a TabControl");
            base.OnInitialized();
            Parent.AddPage(this);
        }
    }
}