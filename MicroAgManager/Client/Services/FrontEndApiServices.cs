using BackEnd.Abstracts;
using Domain.Abstracts;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace FrontEnd.Services
{
    public interface IFrontEndApiServices 
    {
        public Task<Tuple<long, ICollection<T?>>> ProcessQuery<T, TQuery>(string address, TQuery query) where T : BaseModel where TQuery : BaseQuery;
        public Task<long> ProcessCommand<T, TCommand>(string address, TCommand command) where T : BaseModel where TCommand : BaseCommand;
    }
    internal class FrontEndApiServices: IFrontEndApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly FrontEndAuthenticationStateProvider _auth;

        public FrontEndApiServices(HttpClient httpClient, FrontEndAuthenticationStateProvider auth)
        {
            _httpClient = httpClient;
            _auth = auth;
        }

        public async Task<long> ProcessCommand<T, TCommand>(string address, TCommand command) where T : BaseModel where TCommand : BaseCommand
        {
            if (command.Model is null) return -1;
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var commandString = new StringContent(JsonConvert.SerializeObject(command, jsSettings), Encoding.UTF8, "application/json");
            var result = await SendTheRequest(HttpMethod.Post, address, commandString);
            if (result.StatusCode == HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            var list = await result.Content.ReadFromJsonAsync<long>();
            return list;
        }


        public async Task<Tuple<long, ICollection<T?>>> ProcessQuery<T,TQuery>(string address, TQuery query) where  T : BaseModel where TQuery : BaseQuery
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
    }
}
