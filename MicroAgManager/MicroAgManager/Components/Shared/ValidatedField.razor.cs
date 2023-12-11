using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroAgManager.Components.Shared
{
    public partial class ValidatedField<TValue>
    {
        [Parameter] public string Legend { get; set; }
        [Parameter] public string CssClass { get; set; }
        [Parameter] public RenderFragment LegendContent { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public Expression<Func<TValue>> ValidationMessageFor { get; set; }

        bool IsRequired()
        {
            var expression = (MemberExpression)ValidationMessageFor.Body;
            var property = (PropertyInfo)expression.Member;
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            return requiredAttribute != null;
        }
    }
}
