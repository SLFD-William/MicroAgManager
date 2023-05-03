using System.ComponentModel;

namespace MicroAgManagement.Client.Components.Objects.Grid.Filters
{
    public enum EnumCondition
    {
        [Description("Is Equal To")]
        IsEqualTo,

        [Description("Is Not Equal To")]
        IsNotEqualTo,

        [Description("Is null")]
        IsNull,

        [Description("Is not null")]
        IsNotNull
    }
}
