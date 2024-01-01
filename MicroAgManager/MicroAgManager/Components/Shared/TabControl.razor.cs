using Microsoft.AspNetCore.Components;

namespace MicroAgManager.Components.Shared
{
    public partial class TabControl:ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public Action? TabSelected { get; set; }
        [Parameter] public Dictionary<string, TabPage?> SelectedTab { get; set; }
        [Parameter] public string SelectedTabPageKey { get; set; }
        public TabPage? ActivePage { get; set; }
        List<TabPage> Pages = new List<TabPage>();
        internal void AddPage(TabPage tabPage, int tabLocation=-1)
        {
            if(tabLocation<=0 || tabLocation>Pages.Count)
               Pages.Add(tabPage);
            else
                Pages.Insert(tabLocation-1,tabPage);
            StateHasChanged();
        }
        public string GetButtonClass(TabPage page)
        {
            return page == ActivePage ? "active" : string.Empty;
        }

        public void ActivatePage(TabPage page)
        {
            ActivePage = page;
            TabSelected?.Invoke();
            SelectedTab[SelectedTabPageKey]=ActivePage;
            StateHasChanged();
        }
    }
}
