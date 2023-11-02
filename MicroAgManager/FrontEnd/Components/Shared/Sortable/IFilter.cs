using System.Linq.Expressions;

namespace FrontEnd.Components.Shared.Sortable
{
    public interface IFilter<TableItem>
    {
        /// <summary>
        /// Get Filter Expression
        /// </summary>
        /// <returns></returns>
        Expression<Func<TableItem, bool>> GetFilter();

        public string SuperSearchText { get; set; }
    }
}
