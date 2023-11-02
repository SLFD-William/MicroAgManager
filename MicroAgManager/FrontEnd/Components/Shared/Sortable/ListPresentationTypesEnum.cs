using System.ComponentModel;

namespace FrontEnd.Components.Shared.Sortable
{
    public enum ListPresentationTypesEnum
    {
        Grid,
        HorizontalScroll,
        [Description("Card")]
        HorizontalFlex,
        Table,
        Masonry
    }
}
