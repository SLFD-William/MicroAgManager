using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FrontEnd.Components.Shared.Sortable
{
    public partial class ListTemplate<TItem> : SortableDataComponent<TItem>
    {
        //[CascadingParameter] private NavigationManager navigationManager { get; set; }
        //[CascadingParameter] private IModalService modalService { get; set; }
        [Parameter] public ListPresentationTypesEnum ListType { get { return _ListType; } set { _ListType = value; ListTypeUpdated?.Invoke(_ListType); StateHasChanged(); } }
        [Parameter] public List<ListPresentationTypesEnum> AvailableListTypes { get; set; } = Enum.GetValues<ListPresentationTypesEnum>().ToList();
        [Parameter] public RenderFragment MenuBar { get; set; }
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> TableRowTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> TableRowDetailTemplate { get; set; }
        [Parameter] public RenderFragment CountTemplate { get; set; }
        [Parameter] public RenderFragment CommandButtonsTemplate { get; set; }
        [Parameter] public bool ShowTableActionColumn { get; set; } = false;
        [Parameter] public bool ShowViewSelector { get; set; } = false;
        [Parameter] public bool ShowHeader { get; set; } = false;
        [Parameter] public string NewItemName { get; set; }
        [Parameter] public bool OnlyAddInMenuBar { get; set; } = false;
        [Parameter] public Action<ListPresentationTypesEnum> ListTypeUpdated { get; set; }

        public string FilterText { get; set; }
        private ListPresentationTypesEnum _ListType = ListPresentationTypesEnum.Table;
        private void SearchKeyUp(KeyboardEventArgs e) => UpdateSearchTerm();
        private bool alternateRow = false;
        private void ToggleAlternateRow() { alternateRow = !alternateRow; }
        private void ToggleHeader() { ShowHeader = !ShowHeader; StateHasChanged(); }
        //async public Task<ModalResult> ShowEditor<TComponent>(string title, ModalParameters parameters) where TComponent : ComponentBase
        //{
        //    var options = new ModalOptions() { HideCloseButton = true, Class = "ModalInstance__Editor" };
        //    return await modalService.Show<TComponent>(title, parameters, options).Result;
        //}
        //async public Task<ModalResult> GetConfirmation(string title, ModalParameters parameters)
        //{
        //    var options = new ModalOptions() { HideCloseButton = true };
        //    return await modalService.Show<Confirm>(title, parameters, options).Result;
        //}
        string ListTypeClass => $"ListTemplate--{ListType}";
    }

}