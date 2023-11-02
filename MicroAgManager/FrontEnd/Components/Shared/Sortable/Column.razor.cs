using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Linq.Expressions;
using Domain;

namespace FrontEnd.Components.Shared.Sortable
{
    public partial class Column<TableItem> : IColumn<TableItem>
    {
        /// <summary>
        /// Parent Table
        /// </summary>
        [CascadingParameter(Name = "SortableDataContainer")] public ISortableDataComponent<TableItem> SortableDataContainer { get; set; }



        private string _title;

        /// <summary>
        /// Title (Optional, will use Field Name if null)
        /// </summary>
        [Parameter]
        public string Title
        {
            get { return _title ?? Field.GetPropertyMemberInfo()?.Name; }
            set { _title = value; }
        }

        /// <summary>
        /// Width auto|value|initial|inherit
        /// </summary>
        [Parameter] public string Width { get; set; }

        /// <summary>
        /// Column can be sorted
        /// </summary>
        [Parameter] public bool Sortable { get; set; }

        /// <summary>
        /// Column can be filtered
        /// </summary>
        [Parameter] public bool Filterable { get; set; }

        /// <summary>
        /// Normal Item Template
        /// </summary>
        [Parameter] public RenderFragment<TableItem> Template { get; set; }

        /// <summary>
        /// Edit Mode Item Template
        /// </summary>
        [Parameter] public RenderFragment<TableItem> EditTemplate { get; set; }

        /// <summary>
        /// Place custom controls which implement IFilter
        /// </summary>
        [Parameter] public RenderFragment<Column<TableItem>> CustomIFilters { get; set; }

        /// <summary>
        /// Field which this column is for<br />
        /// Required when Sortable = true<br />
        /// Required when Filterable = true
        /// </summary>
        [Parameter] public Expression<Func<TableItem, object>> Field { get; set; }

        ///// <summary>
        ///// Horizontal alignment
        ///// </summary>
        //[Parameter]
        //public Align Align { get; set; }

        /// <summary>
        /// Set the format for values if no template
        /// </summary>
        [Parameter] public string Format { get; set; }

        /// <summary>
        /// Column CSS Class
        /// </summary>
        [Parameter] public string Class { get; set; }

        /// <summary>
        /// Filter expression
        /// </summary>
        public Expression<Func<TableItem, bool>> Filter { get; set; }

        [Parameter] public bool UseInSuperSearch { get; set; }
        /// <summary>
        /// Direction of default sorting
        /// </summary>
        [Parameter] public bool? DefaultSortDescending { get; set; }

        /// <summary>
        /// Direction of default sorting
        /// </summary>
        [Parameter] public bool? DefaultSortColumn { get; set; }

        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Direction of sorting
        /// </summary>
        public bool SortDescending { get; set; }


        /// <summary>
        /// Filter Panel is open
        /// </summary>
        public bool FilterOpen { get; private set; }

        /// <summary>
        /// Column Data Type
        /// </summary>
        [Parameter] public Type Type { get; set; }

        [Parameter] public bool SortOnServer { get; set; } = true;

        /// <summary>
        /// Filter Icon Element
        /// </summary>
        public ElementReference FilterRef { get; set; }

        /// <summary>
        /// Currently applied Filter Control
        /// </summary>
        public IFilter<TableItem> FilterControl { get; set; }

        protected override void OnInitialized()
        {
            SortableDataContainer.AddColumn(this);

            if (DefaultSortDescending.HasValue)
                SortDescending = DefaultSortDescending.Value;
        }

        protected override void OnParametersSet()
        {
            if ((Sortable && Field == null) || (Filterable && Field == null))
                throw new InvalidOperationException($"Column {Title} Property parameter is null");

            if (Title == null && Field == null)
                throw new InvalidOperationException("A Column has both Title and Property parameters null");

            if (Type == null)
                //set the type of the column from the Field
                Type = Field.GetPropertyMemberInfo().GetMemberUnderlyingType();
        }

        /// <summary>
        /// Opens/Closes the Filter Panel
        /// </summary>
        public void ToggleFilter()
        {
            FilterOpen = !FilterOpen;
            SortableDataContainer.Refresh();
        }

        public void SortBy(MouseEventArgs args)
        {
            if (Sortable)
                SortableDataContainer.SortBy(this, args);
        }

        /// <summary>
        /// Render a default value if no template
        /// </summary>
        /// <param name="data">data row</param>
        /// <returns></returns>
        public string Render(TableItem data)
        {
            if (data == null || Field == null)
                return string.Empty;

            if (renderCompiled == null)
                renderCompiled = Field.Compile();

            var value = renderCompiled.Invoke(data);

            if (value == null)
                return string.Empty;

            if (string.IsNullOrEmpty(Format))
                return value.ToString();

            return string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", value);
        }

        /// <summary>
        /// Save compiled renderCompiled property to avoid repeated Compile() calls
        /// </summary>
        private Func<TableItem, object> renderCompiled;
    }
}