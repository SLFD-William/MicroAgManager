
using System.Text.Json.Serialization;

namespace MicroAgManager.Client.Data;

public class WeatherData
{
    [JsonPropertyName("queryCost")]
    public int QueryCost { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("resolvedAddress")]
    public string ResolvedAddress { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("tzoffset")]
    public double? TzOffset { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("alerts")]
    public List<object> Alerts { get; set; }

    [JsonPropertyName("stations")]
    public Dictionary<string, Station> Stations { get; set; }

    [JsonPropertyName("currentConditions")]
    public CurrentConditions CurrentConditions { get; set; }
    [JsonPropertyName("days")]
    public List<DayInfo> Forecast { get; set; }
}

public class Station
{
    [JsonPropertyName("distance")]
    public double? Distance { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("useCount")]
    public int UseCount { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("quality")]
    public double? Quality { get; set; }

    [JsonPropertyName("contribution")]
    public double? Contribution { get; set; }
}

public class CurrentConditions
{
    [JsonPropertyName("datetime")]
    public string Datetime { get; set; }

    [JsonPropertyName("datetimeEpoch")]
    public int DatetimeEpoch { get; set; }

    [JsonPropertyName("temp")]
    public double? Temp { get; set; }

    [JsonPropertyName("feelslike")]
    public double? FeelsLike { get; set; }

    [JsonPropertyName("humidity")]
    public double? Humidity { get; set; }

    [JsonPropertyName("dew")]
    public double? Dew { get; set; }

    [JsonPropertyName("precip")]
    public double? Precip { get; set; }

    [JsonPropertyName("precipprob")]
    public double? PrecipProb { get; set; }

    [JsonPropertyName("snow")]
    public double? Snow { get; set; }

    [JsonPropertyName("snowdepth")]
    public double? SnowDepth { get; set; }

    [JsonPropertyName("windspeed")]
    public double? WindSpeed { get; set; }

    [JsonPropertyName("winddir")]
    public double? WindDir { get; set; }

    [JsonPropertyName("pressure")]
    public double? Pressure { get; set; }

    [JsonPropertyName("visibility")]
    public double? Visibility { get; set; }

    [JsonPropertyName("cloudcover")]
    public double? CloudCover { get; set; }

    [JsonPropertyName("solarradiation")]
    public double? SolarRadiation { get; set; }

    [JsonPropertyName("solarenergy")]
    public double? SolarEnergy { get; set; }

    [JsonPropertyName("uvindex")]
    public double? UvIndex { get; set; }

    [JsonPropertyName("conditions")]
    public string Conditions { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("stations")]
    public List<string> Stations { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("sunrise")]
    public string Sunrise { get; set; }

    [JsonPropertyName("sunriseEpoch")]
    public double? SunriseEpoch { get; set; }

    [JsonPropertyName("sunset")]
    public string Sunset { get; set; }

    [JsonPropertyName("sunsetEpoch")]
    public double? SunsetEpoch { get; set; }

    [JsonPropertyName("moonphase")]
    public double? MoonPhase { get; set; }
}

public class DayInfo    
{
    [JsonPropertyName("DateTime")]
    public string DateTime { get; set; }

    [JsonPropertyName("DateTimeEpoch")]
    public int DateTimeEpoch { get; set; }

    [JsonPropertyName("Tempmax")]
    public double? Tempmax { get; set; }

    [JsonPropertyName("Tempmin")]
    public double? Tempmin { get; set; }

    [JsonPropertyName("Temp")]
    public double? Temp { get; set; }

    [JsonPropertyName("Feelslikemax")]
    public double? Feelslikemax { get; set; }

    [JsonPropertyName("Feelslikemin")]
    public double? Feelslikemin { get; set; }

    [JsonPropertyName("Feelslike")]
    public double? Feelslike { get; set; }

    [JsonPropertyName("Dew")]
    public double? Dew { get; set; }

    [JsonPropertyName("Humidity")]
    public double? Humidity { get; set; }

    [JsonPropertyName("Precip")]
    public double? Precip { get; set; }

    [JsonPropertyName("Precipprob")]
    public double? Precipprob { get; set; }

    [JsonPropertyName("Precipcover")]
    public double? Precipcover { get; set; }

    [JsonPropertyName("Preciptype")]
    public List<string> Preciptype { get; set; }

    [JsonPropertyName("Snow")]
    public double? Snow { get; set; }

    [JsonPropertyName("Snowdepth")]
    public double? Snowdepth { get; set; }

    [JsonPropertyName("Windgust")]
    public double? Windgust { get; set; }

    [JsonPropertyName("Windspeed")]
    public double? Windspeed { get; set; }

    [JsonPropertyName("Winddir")]
    public double? Winddir { get; set; }

    [JsonPropertyName("Pressure")]
    public double? Pressure { get; set; }

    [JsonPropertyName("Cloudcover")]
    public double? Cloudcover { get; set; }

    [JsonPropertyName("Visibility")]
    public double? Visibility { get; set; }

    [JsonPropertyName("Solarradiation")]
    public double? Solarradiation { get; set; }

    [JsonPropertyName("Solarenergy")]
    public double? Solarenergy { get; set; }

    [JsonPropertyName("Uvindex")]
    public double? Uvindex { get; set; }

    [JsonPropertyName("Severerisk")]
    public double? Severerisk { get; set; }

    [JsonPropertyName("Sunrise")]
    public string Sunrise { get; set; }

    [JsonPropertyName("SunriseEpoch")]
    public double? SunriseEpoch { get; set; }

    [JsonPropertyName("Sunset")]
    public string Sunset { get; set; }

    [JsonPropertyName("SunsetEpoch")]
    public double? SunsetEpoch { get; set; }

    [JsonPropertyName("Moonphase")]
    public double? Moonphase { get; set; }

    [JsonPropertyName("Conditions")]
    public string Conditions { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("Icon")]
    public string Icon { get; set; }

    [JsonPropertyName("Stations")]
    public List<string> Stations { get; set; }

    [JsonPropertyName("Source")]
    public string Source { get; set; }

    [JsonPropertyName("Hours")]
    public List<HourInfo> Hours { get; set; }
}

public class HourInfo
{
    [JsonPropertyName("Datetime")]
    public string Datetime { get; set; }
                
    [JsonPropertyName("DatetimeEpoch")]
    public int DatetimeEpoch { get; set; }

    [JsonPropertyName("Temp")]
    public double? Temp { get; set; }

    [JsonPropertyName("Feelslike")]
    public double? Feelslike { get; set; }

    [JsonPropertyName("Humidity")]
    public double? Humidity { get; set; }

    [JsonPropertyName("Dew")]
    public double? Dew { get; set; }

    [JsonPropertyName("Precip")]
    public double? Precip { get; set; }

    [JsonPropertyName("Precipprob")]
    public double? Precipprob { get; set; }

    [JsonPropertyName("Snow")]
    public double? Snow { get; set; }

    [JsonPropertyName("Snowdepth")]
    public double? Snowdepth { get; set; }

    [JsonPropertyName("Preciptype")]
    public List<string> Preciptype { get; set; }

    [JsonPropertyName("Windgust")]
    public double? Windgust { get; set; }

    [JsonPropertyName("Windspeed")]
    public double? Windspeed { get; set; }

    [JsonPropertyName("Winddir")]
    public double? Winddir { get; set; }

    [JsonPropertyName("Pressure")]
    public double? Pressure { get; set; }

    [JsonPropertyName("Visibility")]
    public double? Visibility { get; set; }

    [JsonPropertyName("Cloudcover")]
    public double? Cloudcover { get; set; }

    [JsonPropertyName("Solarradiation")]
    public double? Solarradiation { get; set; }

    [JsonPropertyName("Solarenergy")]
    public double? Solarenergy { get; set; }

    [JsonPropertyName("Uvindex")]
    public double? Uvindex { get; set; }

    [JsonPropertyName("Severerisk")]
    public double? Severerisk { get; set; }

    [JsonPropertyName("Conditions")]
    public string Conditions { get; set; }

    [JsonPropertyName("Icon")]
    public string Icon { get; set; }

    [JsonPropertyName("Stations")]
    public List<string> Stations { get; set; }

    [JsonPropertyName("Source")]
    public string Source { get; set; }
}

