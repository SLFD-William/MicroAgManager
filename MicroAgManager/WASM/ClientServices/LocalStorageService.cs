using Client.Interfaces;
using Microsoft.JSInterop;

namespace MicroAgManager.Client.ClientServices
{
    public class LocalStorageService : ILocalStorage
    {
        private readonly IJSRuntime _js;
        private const string OFFLINE_TOKEN = "offlineTokensCache";
        private const string OfflineClaimsKey = "offlineClaimsCache";
        private const string REMOVE_ITEM = "localStorage.removeItem";
        private const string SET_ITEM = "localStorage.setItem";
        private const string GET_ITEM = "localStorage.getItem";
        public LocalStorageService(IJSRuntime js)
        {
            _js = js;
        }

        async public Task<string> GetOfflineTokenKey() =>
            await _js.InvokeAsync<string>(GET_ITEM, OFFLINE_TOKEN).ConfigureAwait(false);
        async public Task SetOfflineTokenKey(string key) =>
            await _js.InvokeVoidAsync(SET_ITEM, OFFLINE_TOKEN, key);
        async public Task RemoveOfflineTokenKey() =>
            await _js.InvokeVoidAsync(REMOVE_ITEM, OFFLINE_TOKEN);
    }
}
