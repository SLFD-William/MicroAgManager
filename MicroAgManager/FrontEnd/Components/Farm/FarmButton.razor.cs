using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Weather;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Components.Farm
{
    public partial class FarmButton:DataComponent<FarmLocationModel>
    {
        [CascadingParameter] FarmLocationModel? Farm { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? ContainerAttributes { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }

        [Parameter] public long? farmId { get; set; }
        [Parameter] public string CustomButtonStyle { get; set; }
        [Parameter] public bool ShowWeather { get; set; } = true;

        private Weather.Weather _weather;
        private FarmLocationModel? farm;
        private string PointerClass()
        {
            var styleClass = OnClick.HasDelegate ? "pointer" : string.Empty;
            var colorClass = CustomButtonStyle is null ? string.Empty : " customColor";

            return $"{styleClass} {colorClass}".Trim();


        }
        private string CustomButtonStyles() => CustomButtonStyle is null ? string.Empty : CustomButtonStyle;
        public override async Task FreshenData()
        {
            if (Farm is not null)
                farm = Farm;
            if (Farm is null && farmId.HasValue)
                farm = await app.dbContext.Farms.FindAsync(farmId);
        }
    }
}
