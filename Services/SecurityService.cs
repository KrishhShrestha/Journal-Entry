using System.Threading.Tasks;
using Microsoft.Maui.Storage;



namespace Journal_Entry.Services
{
    public class SecurityService
    {
        private const string PinKey = "user_pin";
        private const string UsernameKey = "username";

        // ---------- PIN ----------
        public async Task SavePinAsync(string pin)
        {
            await SecureStorage.SetAsync(PinKey, pin);
        }

        public async Task<string?> GetPinAsync()
        {
            return await SecureStorage.GetAsync(PinKey);
        }

        public async Task<bool> IsPinSetAsync()
        {
            var pin = await GetPinAsync();
            return !string.IsNullOrEmpty(pin);
        }

        // ---------- USERNAME ----------
        public void SaveUsername(string username)
        {
            Preferences.Set(UsernameKey, username);
        }

        public string GetUsername()
        {
            return Preferences.Get(UsernameKey, string.Empty);
        }
    }
}
