using Microsoft.AspNetCore.Components.Web;

namespace FrontEnd.Components.Shared.Sortable
{
    public interface ISortableDataComponent<TableItem>
    {
        string SearchTerm { get; set; }
        bool ShowSearchDetail { get; set; }
        void Update();
        void Refresh();
        void FirstPage();
        void SortBy(Column<TableItem> column, MouseEventArgs args);
        void AddColumn(Column<TableItem> column);

    }
}
