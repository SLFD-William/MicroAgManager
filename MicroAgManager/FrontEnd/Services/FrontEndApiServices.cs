using BackEnd.Abstracts;
using Domain.Abstracts;
using Domain.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FrontEnd.Services
{
    public interface IFrontEndApiServices 
    {
        public Task<Tuple<long, ICollection<T?>>> ProcessQuery<T, TQuery>(string address, TQuery query) where T : class where TQuery : BaseQuery;
        public Task<long> ProcessCommand<T, TCommand>(string address, TCommand command) where T : BaseModel where TCommand : BaseCommand;
        public Task<BingLocationResponse?> GetClosestAddress(double latitude, double longitude);
        public Task<BingLocationResponse?> GetClosestGeoLocation(FarmLocationModel farm);
        public Task<WeatherData> GetWeather(double Latitued, double Longitude,bool force=false);
    }
    internal class FrontEndApiServices: IFrontEndApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly FrontEndAuthenticationStateProvider _auth;
        private const string BingMapsAPIKey = "AveuKfTkHgt1rHA0CQdugzV00kfhicCACDHBHoSH4ddPK24p8W1_hyPUnBmjUgSG";
        private const string WeatherAPIKey = "UX43BL6B8S3P6KE6PGRA6PUSD";
        private Dictionary<string,Tuple<WeatherData,DateTime>> weatherTracking=new Dictionary<string, Tuple<WeatherData, DateTime>>();
        
        
        public async Task<BingLocationResponse?> GetClosestAddress(double latitude, double longitude)
        {

            string url = $"https://dev.virtualearth.net/REST/v1/Locations/{latitude},{longitude}?key={BingMapsAPIKey} ";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return System.Text.Json.JsonSerializer.Deserialize<BingLocationResponse>(json, options);
        }
        public async Task<BingLocationResponse?> GetClosestGeoLocation(FarmLocationModel farm)
        {
            string url = $"https://dev.virtualearth.net/REST/v1/Locations/{farm.Country}/{farm.State}/{farm.Zip}/{farm.City}/{farm.StreetAddress}?&maxResults={1}&key={BingMapsAPIKey}";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return System.Text.Json.JsonSerializer.Deserialize<BingLocationResponse>(json, options);
        }
        public FrontEndApiServices(HttpClient httpClient, FrontEndAuthenticationStateProvider auth)
        {
            _httpClient = httpClient;
            _auth = auth;
        }

        public async Task<long> ProcessCommand<T, TCommand>(string address, TCommand command) where T : BaseModel where TCommand : BaseCommand
        {
            if (command is null) return -1;
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var commandString = new StringContent(JsonConvert.SerializeObject(command, jsSettings), Encoding.UTF8, "application/json");
            var result = await SendTheRequest(HttpMethod.Post, address, commandString);
            if (result.StatusCode == HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            var list = await result.Content.ReadFromJsonAsync<long>();
            return list;
        }


        public async Task<Tuple<long, ICollection<T?>>> ProcessQuery<T,TQuery>(string address, TQuery query) where  T : class where TQuery : BaseQuery
        {
            var queryString = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            var result = await SendTheRequest(HttpMethod.Post, address, queryString);
            if (result.StatusCode == HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            var list = await result.Content.ReadFromJsonAsync<Tuple<long, ICollection<T?>>>();
            return list;
        }
        private async Task<HttpResponseMessage> SendTheRequest(HttpMethod method, string address, StringContent? content)
        {
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.Forbidden };
            //await LogApiMessage(method, address, content, response.StatusCode);
            //if (_appState.AccessLevel <= TenantAccessLevel.LocalStorage)
            //    return response;
            //using (var db = await _appState.dbContext())
            //{
            //    foreach (var apiMessage in db.ApiMessageLog)
            //    {
            //        method = JsonConvert.DeserializeObject<HttpMethod>(apiMessage.Method);
            //        content = JsonConvert.DeserializeObject<StringContent?>(apiMessage.Content);

                    var requestMsg = new HttpRequestMessage(method,address);
                    requestMsg.Content = content;
                    for (int i = 0; i < 2; i++)
                    {
                        requestMsg.Headers.Add("Authorization", "Bearer " + await _auth.GetJWT());
                        response = await _httpClient.SendAsync(requestMsg);
                        if(response.StatusCode==HttpStatusCode.OK) break;
                        if (response.StatusCode != HttpStatusCode.Unauthorized)
                            continue;
                        await _auth.RefreshToken();
                    }
                    //if (response.StatusCode == HttpStatusCode.OK)
                    //    db.ApiMessageLog.Remove(apiMessage);
                    //else
                    //    break;
                //}
                //await db.SaveChangesAsync();
            //}
            return response;
        }

        public async Task<WeatherData> GetWeather(double latitude, double longitude, bool force = false)
        {
            var weatherKey = $"{latitude},{longitude}";
            Tuple<WeatherData, DateTime> currentWeatherData = weatherTracking.ContainsKey(weatherKey) ? weatherTracking[weatherKey] : null;

            if (!force && currentWeatherData is not null && DateTime.Now.Subtract(currentWeatherData.Item2).TotalMinutes<60)
                return currentWeatherData.Item1;

            string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{latitude}%2C{longitude}?unitGroup=us&maxDistance=16093&key={WeatherAPIKey}&contentType=json";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var currentWeather = new Tuple<WeatherData, DateTime>(System.Text.Json.JsonSerializer.Deserialize<WeatherData>(json, options), DateTime.Now);
            if (!weatherTracking.ContainsKey(weatherKey)) weatherTracking.Add(weatherKey,currentWeather);
            weatherTracking[weatherKey] = currentWeather;
            return currentWeather.Item1;
        }
    }
}
