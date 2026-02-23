using BusinessLayer.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;

namespace Web_App___Blazor.Services
{
    public class UserSessionService
    {
        private readonly ProtectedSessionStorage _storage;
        private const string Key = "current_user";

        public UserDTO? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;
        public int UserId => CurrentUser?.Id ?? 0;
        public int RoleId => CurrentUser?.RoleId ?? 0;

        // Stranice se pretplaćuju na ovaj event i re-renderuju se kada sesija bude učitana
        public event Action? OnChange;

        public UserSessionService(ProtectedSessionStorage storage)
        {
            _storage = storage;
        }

        public async Task LoginAsync(UserDTO user)
        {
            CurrentUser = user;
            await _storage.SetAsync(Key, JsonSerializer.Serialize(user));
            OnChange?.Invoke();
        }

        public async Task LoadFromStorageAsync()
        {
            if (CurrentUser != null) return;

            try
            {
                var result = await _storage.GetAsync<string>(Key);
                if (result.Success && result.Value != null)
                {
                    CurrentUser = JsonSerializer.Deserialize<UserDTO>(result.Value);
                    OnChange?.Invoke();
                }
            }
            catch
            {
                // Storage nije dostupan (npr. pre JS inicijalizacije)
            }
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            await _storage.DeleteAsync(Key);
            OnChange?.Invoke();
        }
    }
}