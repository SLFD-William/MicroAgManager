using System.Linq.Expressions;

namespace FrontEnd.Components.Shared.Sortable
{
    public interface IColumn<TableItem>
    {
        ISortableDataComponent<TableItem> SortableDataContainer { get; set; }
        void ToggleFilter();
        IFilter<TableItem> FilterControl { get; set; }
        Expression<Func<TableItem, object>>? Field { get; set; }
        Expression<Func<TableItem, bool>>? Filter { get; set; }
        bool SortDescending { get; set; }
        bool SortOnServer { get; set; }
        Type Type { get; }
    }
}
