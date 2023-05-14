using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using Domain;

namespace FrontEnd.Components.Shared.Sortable
{
    public partial class SortableDataComponent<TItem> : ComponentBase,ISortableDataComponent<TItem>
    {
        private const int DEFAULT_PAGE_SIZE = 10;

        [Parameter]
        public RenderFragment ColumnContent { get; set; }

        public List<Column<TItem>> Columns { get; } = new List<Column<TItem>>();

        [Parameter]
        public bool UseVirtualization { get; set; } = true;

        [Parameter]
        public IQueryable<TItem> ItemsQueryable { get; set; }

        [Parameter]
        public int OverscanCount { get; set; }

        [Parameter]
        public int ItemSize { get; set; }

        private Dictionary<string, object> _FilterColumns = new Dictionary<string, object>();
        [Parameter]
        public Dictionary<string, object> FilterColumns
        {
            get
            { return _FilterColumns; }
            set
            {
                _FilterColumns = value;
            }
        }



        private OrderedDictionary _SortColumns = new OrderedDictionary();

        [Parameter]
        public OrderedDictionary SortColumns
        {
            get
            { return _SortColumns; }
            set
            {
                _SortColumns = value;
                SetSortOrderIndicator();
            }
        }


        [Parameter]
        public IEnumerable<TItem> Items { get; set; }


        public IEnumerable<TItem> FilteredItems { get; private set; }

        public IEnumerable<TItem> SelectedItems { get; set; }=new List<TItem>();
        public void AddToSelectedItems(TItem item)
        {
            if (!Selectable) return;
            var list = SelectedItems.ToList();
            list.Add(item);
            SelectedItems = MultiSelect ? list : new List<TItem> {{item}};
            OnSelectionChange?.Invoke();
        }
        public void RemoveFromSelectedItems(TItem item)
        {
            if (!Selectable) return;
            var list = SelectedItems.ToList();
            list.Remove(item);
            SelectedItems = list;
            OnSelectionChange?.Invoke();
        }
        public bool IsItemSelected(TItem item)=> SelectedItems.Contains(item);
        
        [Parameter] public Action? OnSelectionChange { get; set; }
        [Parameter] public bool MultiSelect { get; set; }=false;
        [Parameter] public bool Selectable { get; set; } = false;





        [Parameter]
        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

        [Parameter]
        public bool ColumnReorder { get; set; }

        [Parameter] public string SearchTerm { get; set; }
        public int PageNumber { get; private set; }


        private bool _ShowSearchDetail = false;
        [Parameter]
        public bool ShowSearchDetail
        {
            get { return _ShowSearchDetail; }
            set
            {
                if (_ShowSearchDetail == value)
                    return;
                _ShowSearchDetail = value;

                foreach (var col in Columns)
                    col.Filter = null;

                FilterColumns.Clear();

                _FilterReset = false;

                SearchTerm = string.Empty;
                OnSearchDetailChange?.Invoke(_ShowSearchDetail);
                UpdateSearchTerm();
            }
        }

        public int TotalCount { get; private set; }


        public bool IsEditMode { get; private set; }


        public int TotalPages => PageSize <= 0 ? 1 : (TotalCount + PageSize - 1) / PageSize;
        public void AddColumn(Column<TItem> column)
        {
            Columns.Add(column);
            Refresh();
        }

        public void RemoveColumn(Column<TItem> column)
        {
            Columns.Remove(column);
            Refresh();
        }

        public void Refresh()
        {
            StateHasChanged();
        }
        public void FirstPage()
        {
            if (PageNumber != 0)
            {
                PageNumber = 0;
                Update();
            }
        }
        public void Update()
        {
            FilteredItems = GetData();
            Refresh();
        }
        private void SetSortOrderIndicator()
        {
            foreach (Column<TItem> column in Columns)
                column.SortOrder = 0;

            int index = 1;
            foreach (Column<TItem> column in SortColumns.Values)
            {
                if (Columns.Any())
                {
                    Columns.First(x => x.Field.GetPropertyMemberInfo()?.Name == column.Field.GetPropertyMemberInfo()?.Name).SortOrder = index;
                    index += 1;
                }
            }
        }
        public void SortBy(Column<TItem> column, MouseEventArgs args)
        {
            var fieldName = column.Field.GetPropertyMemberInfo()?.Name;
            if (args.CtrlKey && SortColumns.Contains(fieldName))
            {
                SortColumns.Remove(fieldName);
                SetSortOrderIndicator();
                Update();
                return;
            }
            if (!SortColumns.Contains(fieldName))
            {
                if (!args.ShiftKey)
                    SortColumns.Clear();

                SortColumns.Add(fieldName, column);
            }
            (SortColumns[fieldName] as Column<TItem>).SortDescending = !(SortColumns[fieldName] as Column<TItem>).SortDescending;
            SetSortOrderIndicator();
            Update();
            OnSortChange?.Invoke();
        }
        private bool _FilterReset = false;
        protected override void OnParametersSet()
        {
            Update();
        }

        private IQueryable<TItem> GetFilteredData()
        {

            var queriedItems = ItemsQueryable.ToList();
            if (!string.IsNullOrWhiteSpace(SearchTerm) && Columns.Any(_ => _.Filter != null))
                queriedItems.Clear();

            foreach (var item in Columns)
            {
                if (item is null) continue;

                var fieldName = item.Field.GetPropertyMemberInfo()?.Name ?? string.Empty;

                if (FilterColumns.ContainsKey(fieldName) && item.Filter == null)
                    FilterColumns.Remove(fieldName);
                if (!FilterColumns.ContainsKey(fieldName) && item.Filter != null)
                    FilterColumns.Add(fieldName, item);

                if (item.Filter != null)
                {
                    if (!string.IsNullOrWhiteSpace(SearchTerm))
                        queriedItems.AddRange(Items.AsQueryable().Where(item.Filter).ToList());
                    else
                        queriedItems = queriedItems.AsQueryable().Where(item.Filter).ToList();
                }
            }
            return queriedItems.Distinct().AsQueryable();
        }
        protected void UpdateSearchTerm()
        {
            OnSearchTermChange?.Invoke(SearchTerm);
            if (!Columns.Any())
                return;

            Columns.First().SortableDataContainer.Update();

        }
        private IEnumerable<TItem> GetData()
        {
            if (Items != null || ItemsQueryable != null)
            {
                if (Items != null)
                {
                    ItemsQueryable = Items.AsQueryable();
                }

                if (Columns.Any() && FilterColumns.Any() && !_FilterReset)
                {
                    _FilterReset = true;
                    if (!Columns.Any(_ => _.Filter != null))
                    {
                        foreach (var col in FilterColumns)
                        {

                            var filteredColumn = Columns.First(_ => _.Field.GetPropertyMemberInfo()?.Name == col.Key);
                            filteredColumn.Filter = ((Column<TItem>)col.Value).Filter;
                        }
                    }
                }

                ItemsQueryable = GetFilteredData();

                TotalCount = ItemsQueryable.Count();

                ApplySorting();

                // if PageSize is zero, we return all rows and no paging
                if (PageSize <= 0)
                    return ItemsQueryable.ToList();
                else
                    return ItemsQueryable.Skip(PageNumber * PageSize).Take(PageSize).ToList();
            }

            return Items;
        }

        private void ApplySorting()
        {

            foreach (Column<TItem> sortColumn in SortColumns.Values)
                ApplySort(sortColumn);

            var defaultSort = Columns.FirstOrDefault(_ => _.DefaultSortColumn ?? false);
            if (SortColumns.Values.Count == 0 && defaultSort != null)
                ApplySort(defaultSort);


        }

        private void ApplySort(Column<TItem> sortColumn)
        {
            var propertyType = sortColumn.Field.GetPropertyMemberInfo()?.GetMemberUnderlyingType();
            var fieldName = sortColumn.Field.GetPropertyMemberInfo()?.Name;
            var nullable = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
                ItemsQueryable = Utilities.ApplyOrder(ItemsQueryable, $"{fieldName}.HasValue", (sortColumn.SortOrder <= 1 ? "OrderByDescending" : "ThenByDescending"));

            if (sortColumn.SortDescending)
                if (nullable)
                    ItemsQueryable = Utilities.ApplyOrder(ItemsQueryable, fieldName, "ThenByDescending");
                else
                    ItemsQueryable = Utilities.ApplyOrder(ItemsQueryable, fieldName, (sortColumn.SortOrder <= 1 ? "OrderByDescending" : "ThenByDescending"));
            else
                if (nullable)
                ItemsQueryable = Utilities.ApplyOrder(ItemsQueryable, fieldName, "ThenBy");
            else
                ItemsQueryable = Utilities.ApplyOrder(ItemsQueryable, fieldName, (sortColumn.SortOrder <= 1 ? "OrderBy" : "ThenBy"));
        }

        [Parameter] public Action AddItemClicked { get; set; }
        private int _ServerFilterValue = 0;
        [Parameter]
        public int ServerFilterValue
        {
            get { return _ServerFilterValue; }
            set
            {
                if (value != _ServerFilterValue)
                {
                    _ServerFilterValue = value;
                    OnServerFilterChange?.Invoke(_ServerFilterValue);
                }
            }
        }
        [Parameter] public Type ServerFilterEnum { get; set; }
        [Parameter] public Action<int> OnServerFilterChange { get; set; }
        [Parameter] public Action OnSortChange { get; set; }
        [Parameter] public Action<string> OnSearchTermChange { get; set; }
        [Parameter] public Action<bool> OnSearchDetailChange { get; set; }

        [Parameter] public bool admin { get; set; } = false;
    }

}
