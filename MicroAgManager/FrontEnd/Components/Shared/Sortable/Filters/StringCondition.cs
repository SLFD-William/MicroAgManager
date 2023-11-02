using System.ComponentModel;

namespace MicroAgManagement.Client.Components.Objects.Grid.Filters
{
    public enum StringCondition
    {
        [Description("Contains")]
        Contains,

        [Description("Does not contain")]
        DoesNotContain,

        [Description("Starts with")]
        StartsWith,

        [Description("Ends with")]
        EndsWith,

        [Description("Is equal to")]
        IsEqualTo,

        [Description("Is not equal to")]
        IsNotEqualTo,

        [Description("Is null or empty")]
        IsNullOrEmpty,

        [Description("Is not null or empty")]
        IsNotNulOrEmpty
    }
}
