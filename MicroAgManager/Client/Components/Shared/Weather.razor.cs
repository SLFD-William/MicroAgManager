using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace FrontEnd.Components.Shared
{
    public partial class Weather:IAsyncDisposable
    {
        [Parameter] public double? Latitude { get; set; }
        [Parameter] public double? Longitude { get; set; }
        [Inject] private IFrontEndApiServices api { get; set; }
        private WeatherData weather { get; set; }

        private Timer weatherUpdateTimer { get; set; }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (Latitude.HasValue && Longitude.HasValue && weather is null)
            {
                weatherUpdateTimer= new Timer(async _ => { await RefreshWeather(); }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            }
        }
        private async Task RefreshWeather()
        {
            weather = await api.GetWeather(Latitude.Value, Longitude.Value);
            StateHasChanged();
        }
        private static string MapToFontAwesome(string weatherIcon)=> weatherIcon switch
            {   
                "snow"=> "fas fa-snowflake",
                "snow-showers-day" => "fas fa-cloud-snow",
                "snow-showers-night" => "fas fa-cloud-snow",
                "rain" => "fas fa-cloud-rain",
                "showers-day" => "fas fa-cloud-sun-rain",
                "showers-night" => "fas fa-cloud-moon-rain",
                "thunder-rain" => "fas fa-cloud-bolt",
                "thunder-showers-day" => "fas fa-cloud-bolt-sun",
                "thunder-showers-night" => "fas fa-cloud-bolt-moon",
                "fog" => "fas fa-cloud-fog",
                "wind" => "fas fa-wind",
                "cloudy" => "fas fa-clouds",
                "partly-cloudy-day" => "fas fa-cloud-sun",
                "partly-cloudy-night" => "fas fa-cloud-moon",
                "clear-day" => "fas fa-sun",
                "clear-night" => "fas fa-moon",
                _ => "fas fa-question-circle"
            };

        public ValueTask DisposeAsync()
        {
            weatherUpdateTimer?.DisposeAsync();
            return ValueTask.CompletedTask;
        }
    }
}
