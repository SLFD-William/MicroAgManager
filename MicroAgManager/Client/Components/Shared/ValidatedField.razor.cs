using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace FrontEnd.Components.Shared
{
    public partial class ValidatedField<TValue>
    {
        [Parameter] public string Legend { get; set; }
        [Parameter] public string CssClass { get; set; }
        [Parameter] public RenderFragment LegendContent { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public Expression<Func<TValue>> ValidationMessageFor { get; set; }
    }
}
