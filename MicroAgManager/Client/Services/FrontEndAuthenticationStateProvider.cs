using BackEnd.Authentication;
using FrontEnd.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

namespace FrontEnd.Services
{
    internal class FrontEndAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorage _localStorage;
        private readonly HttpClient _httpClient;
        public ClaimsPrincipal? User { get; private set; }
        public Guid UserId() => Guid.Parse(User?.FindFirstValue("sub") ?? string.Empty);
        public Guid TenantId() 
        {
            if(Guid.TryParse(User?.FindFirstValue("TenantId") ,out var guid))
                return guid;
            return Guid.Empty;
        }
        

        public FrontEndAuthenticationStateProvider(ILocalStorage localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        private JwtSecurityToken? _currentToken { get; set; }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                for (int i = 0; i < 2; i++)
                {
                    var cachedClaimsJson = await GetJWT();
                    if (_currentToken is null && string.IsNullOrEmpty(cachedClaimsJson)) break;
                    var token = tokenHandler.ReadJwtToken(cachedClaimsJson);
                    user = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt"));
                    if (user.Identity?.IsAuthenticated ?? false) return new AuthenticationState(user);
                    await RefreshToken();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            catch { }
            return new AuthenticationState(user);
        }

        public async Task<string?> GetJWT()
        {
            var userdata = await _localStorage.GetOfflineTokenKey();
            if (!string.IsNullOrWhiteSpace(userdata))
            {
                var dataArray = userdata.Split(';', 2);
                if (dataArray.Length == 2)
                    return dataArray[0];
            }
            return null;
        }
        public async Task<TokenModel> RefreshToken(TokenModel token)
        {
            var result = await _httpClient.PostAsJsonAsync("api/security/refreshtoken", token);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            var newToken = await result.Content.ReadFromJsonAsync<TokenModel>();
            await StoreAuthentication(newToken);
            return newToken;
        }

        public async Task<LoginResult> Login(LoginUserCommand loginRequest) =>
            await GetResult(await _httpClient.PostAsJsonAsync("api/security/login", loginRequest ));

        private async Task<LoginResult> GetResult(HttpResponseMessage? result)
        {
            if (result == null)
                return new LoginResult { success = false, message = "no data" };

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            //result.EnsureSuccessStatusCode();
            var loginResult = await result.Content.ReadFromJsonAsync<LoginResult>();
            if (loginResult is null)
                loginResult = new LoginResult { success = false, message = "no data" };
            if (!loginResult.success)
                loginResult.token = null;

            await StoreAuthentication(loginResult.token);
            return loginResult;
        }
        public async Task Logout()
        {
            User = null;
            _currentToken=null;
            await StoreAuthentication(null);
        }

        virtual public async Task<LoginResult> Register(RegisterUserCommand registerRequest)
        {
            var command = await _httpClient.PostAsJsonAsync("api/security/register", registerRequest );
            return await GetResult(command);
        }


        public async Task RefreshToken()
        {
            var token = new TokenModel();
            var userdata = await _localStorage.GetOfflineTokenKey();
            if (string.IsNullOrWhiteSpace(userdata))
            {
                await StoreAuthentication(null);
                return;
            }
            var dataArray = userdata.Split(';', 3);
            if (dataArray.Length == 3)
            {
                token.jwtBearer = dataArray[0];
                token.refreshToken = dataArray[1];
                token.expiration = DateTime.Parse(dataArray[2]);
            }
            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/security/refreshtoken", new RefreshTokenCommand() { Token=token } );
                var newToken = await result.Content.ReadFromJsonAsync<TokenModel>();
                await StoreAuthentication(newToken);
            }
            catch
            {
                await StoreAuthentication(null);
            }
        }
        private async Task StoreAuthentication(TokenModel? token)
        {
            await _localStorage.RemoveOfflineTokenKey();
            if (token is not null)
                await _localStorage.SetOfflineTokenKey($"{token.jwtBearer};{token.refreshToken};{token.expiration}");

            var state = await GetAuthenticationStateAsync();
            User = state?.User;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

