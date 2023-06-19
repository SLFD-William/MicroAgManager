using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared
{
    public partial class TabControl:ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public Action? PlotSelected { get; set; }
        public TabPage? ActivePage { get; set; }
        List<TabPage> Pages = new List<TabPage>();
        internal void AddPage(TabPage tabPage)
        {
            Pages.Add(tabPage);
            StateHasChanged();
        }
        public string GetButtonClass(TabPage page) => page == ActivePage ? "active" : string.Empty;

        public void ActivatePage(TabPage page)
        {
            ActivePage = ActivePage == page ? null : page;
            StateHasChanged();
        }
    }
}
