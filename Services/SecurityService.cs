using Microsoft.Maui.Storage;

namespace Journal_Entry.Services
{
    public class SecurityService
    {
        private const string PIN_KEY = "userPin";
        private const string USERNAME_KEY = "username";
        private const string AUTH_KEY = "isAuthenticated";

        // Get PIN from SecureStorage
        public async Task<string> GetPinAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(PIN_KEY) ?? "";
            }
            catch
            {
                return "";
            }
        }

        // Save PIN to SecureStorage
        public async Task SetPinAsync(string pin)
        {
            try
            {
                await SecureStorage.SetAsync(PIN_KEY, pin);
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error saving PIN: {ex.Message}");
            }
        }

        // Check if PIN is set
        public async Task<bool> IsPinSetAsync()
        {
            try
            {
                var pin = await SecureStorage.GetAsync(PIN_KEY);
                return !string.IsNullOrEmpty(pin);
            }
            catch
            {
                return false;
            }
        }

        // Get Username from Preferences
        public string GetUsername()
        {
            return Preferences.Get(USERNAME_KEY, "");
        }

        // Save Username to Preferences
        public void SetUsername(string username)
        {
            Preferences.Set(USERNAME_KEY, username);
        }

        // Check if user is authenticated (in-memory flag)
        private bool _isAuthenticated = false;

        public async Task<bool> IsAuthenticatedAsync()
        {
            return _isAuthenticated;
        }

        public async Task SetAuthenticatedAsync(bool isAuthenticated)
        {
            _isAuthenticated = isAuthenticated;
        }

        // Clear authentication on app restart
        public void ClearAuthentication()
        {
            _isAuthenticated = false;
        }

        // Check if PIN exists (alias for IsPinSetAsync)
        public async Task<bool> HasPinAsync()
        {
            return await IsPinSetAsync();
        }
    }
}