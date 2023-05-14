using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FrontEnd.Components.Shared.Sortable
{
    public partial class TableTemplate<TItem> : SortableDataComponent<TItem>
    {
        [Parameter] public RenderFragment MenuBar { get; set; }
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> TableRowTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> TableRowDetailTemplate { get; set; }
        [Parameter] public RenderFragment CountTemplate { get; set; }
        [Parameter] public RenderFragment CommandButtonsTemplate { get; set; }
        [Parameter] public bool ShowTableActionColumn { get; set; } = false;
        [Parameter] public bool ShowViewSelector { get; set; } = false;
        [Parameter] public string NewItemName { get; set; }
        [Parameter] public string Caption { get; set; }
        [Parameter] public bool OnlyAddInMenuBar { get; set; } = false;
        [Parameter] public Action<ListPresentationTypesEnum> ListTypeUpdated { get; set; }

        public string FilterText { get; set; }
        private ListPresentationTypesEnum _ListType = ListPresentationTypesEnum.Table;
        private void SearchKeyUp(KeyboardEventArgs e) => UpdateSearchTerm();
        private void OnRowDoubleClicked(MouseEventArgs e, TItem item)
        { 
            if(SelectedItems.Contains(item))
                RemoveFromSelectedItems(item);
            else
                AddToSelectedItems(item);
        }
        private string SelectedClass(TItem item)=> IsItemSelected(item) ? "selected":"";
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
    }

}