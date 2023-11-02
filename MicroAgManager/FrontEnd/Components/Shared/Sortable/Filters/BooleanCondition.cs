using System.ComponentModel;

namespace FrontEnd.Components.Shared.Sortable.Filters
{
    public enum BooleanCondition
    {
        [Description("True")]
        True,

        [Description("False")]
        False,

        [Description("Is null")]
        IsNull,

        [Description("Is not null")]
        IsNotNull
    }
}
