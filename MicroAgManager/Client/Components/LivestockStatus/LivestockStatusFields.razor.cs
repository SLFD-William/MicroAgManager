using Domain.Constants;
using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockStatus
{
    public partial class LivestockStatusFields
    {
        private readonly static List<string> StatusModes = new List<string> {
            string.Empty,
            LivestockStatusModeConstants.Unchanged,
            LivestockStatusModeConstants.False,
            LivestockStatusModeConstants.True
        };
        [CascadingParameter] public LivestockStatusModel LivestockStatus { get; set; }
    }
}
