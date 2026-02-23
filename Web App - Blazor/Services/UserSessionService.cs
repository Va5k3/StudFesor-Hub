using BusinessLayer.DTOs;

namespace Web_App___Blazor.Services
{
    public class UserSessionService
    {
        public UserDTO? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;
        public int UserId => CurrentUser?.Id ?? 0;
        public int RoleId => CurrentUser?.RoleId ?? 0;

        public void Login(UserDTO user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}