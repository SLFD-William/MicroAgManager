namespace FrontEnd.Interfaces
{
    public interface ILocalStorage
    {
        public Task<string> GetOfflineTokenKey();
        public Task SetOfflineTokenKey(string key);
        public Task RemoveOfflineTokenKey();
    }
}
