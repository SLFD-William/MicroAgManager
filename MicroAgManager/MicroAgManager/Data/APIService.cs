﻿using BackEnd.Abstracts;
using Domain.Abstracts;
using Domain.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicroAgManager.Data
{
    public interface IAPIService
    {
        public Task<string> TestApiResult();
        public Task<Tuple<long, ICollection<T?>>> ProcessQuery<T, TQuery>(string address, TQuery query) where T : class where TQuery : BaseQuery;
        public Task<long> ProcessCommand<T, TCommand>(string address, TCommand command) where T : BaseModel where TCommand : BaseCommand;
        public Task<BingLocationResponse?> GetClosestAddress(double latitude, double longitude);
        public Task<BingLocationResponse?> GetClosestGeoLocation(FarmLocationModel farm);
        public Task<WeatherData> GetWeather(double Latitued, double Longitude, bool force = false);
    }
    public class APIService: IAPIService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private Dictionary<string, Tuple<WeatherData, DateTime>> weatherTracking = new Dictionary<string, Tuple<WeatherData, DateTime>>();
        
        
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        public APIService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<BingLocationResponse?> GetClosestGeoLocation(FarmLocationModel farm)
        {
            var key = _config["BingMapsAPIKey"];
            var queryURL = _config["BingMapsLocationQueryURL"];
            string url = string.Format(queryURL, farm.Country, farm.State, farm.Zip, farm.City, farm.StreetAddress, 1, key);
            using(var http= _httpClientFactory.CreateClient())
            { 
                var response = await http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<BingLocationResponse>(json, options);
            }
        }

        public async Task<BingLocationResponse?> GetClosestAddress(double latitude, double longitude)
        {
            var key = _config["BingMapsAPIKey"];
            var queryURL = _config["BingMapsLatLongQueryURL"];
            string url = string.Format(queryURL, latitude, longitude, key);
            using (var http = _httpClientFactory.CreateClient())
            {
                var response = await http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<BingLocationResponse>(json, options);
            }
        }
        public async Task<WeatherData> GetWeather(double latitude, double longitude, bool force = false)
        {
            var weatherKey = $"{latitude},{longitude}";
            Tuple<WeatherData, DateTime> currentWeatherData = weatherTracking.ContainsKey(weatherKey) ? weatherTracking[weatherKey] : null;

            if (!force && currentWeatherData is not null && DateTime.Now.Subtract(currentWeatherData.Item2).TotalMinutes < 60)
                return currentWeatherData.Item1;

            var key = _config["WeatherAPIKey"];
            var queryURL = _config["WeatherQueryURL"];
            string url = string.Format(queryURL, latitude, longitude, key);
            using (var http = _httpClientFactory.CreateClient())
            {
                var response = await http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                var currentWeather = new Tuple<WeatherData, DateTime>(JsonSerializer.Deserialize<WeatherData>(json, options), DateTime.Now);
                if (!weatherTracking.ContainsKey(weatherKey)) weatherTracking.Add(weatherKey, currentWeather);
                weatherTracking[weatherKey] = currentWeather;
                return currentWeather.Item1;
            }
        }

        public async Task<long> ProcessCommand<T, TCommand>(string address, TCommand command)
            where T : BaseModel
            where TCommand : BaseCommand
        {
            if (command is null) return -1;
                    try { 
            var commandString = new StringContent(JsonSerializer.Serialize(command, _jsonOptions), Encoding.UTF8, "application/json");
            var result = await SendTheRequest(HttpMethod.Post, address, commandString);
            if (result.StatusCode == HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            var list = await result.Content.ReadFromJsonAsync<long>();
            return list;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return -1;
        }

        public async Task<Tuple<long, ICollection<T?>>> ProcessQuery<T, TQuery>(string address, TQuery query)
            where T : class
            where TQuery : BaseQuery
        {
            var queryString = new StringContent(JsonSerializer.Serialize(query, _jsonOptions), Encoding.UTF8, "application/json");
            var result = await SendTheRequest(HttpMethod.Post, address, queryString);
            if (result.StatusCode == HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            return await DataSynchronizer.ParseTheJSON<T>(result);
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

            var requestMsg = new HttpRequestMessage(method, address);
            requestMsg.Content = content;
            var http = _httpClientFactory.CreateClient("API");
            for (int i = 0; i < 1; i++)
            {
            
                response = await http.SendAsync(requestMsg);
                
                if (response.StatusCode == HttpStatusCode.OK) break;
                if (response.StatusCode != HttpStatusCode.Unauthorized)
                    continue;
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

        public async Task<string> TestApiResult()=> await _httpClientFactory.CreateClient("API").GetStringAsync("api/Test");
    }
}
