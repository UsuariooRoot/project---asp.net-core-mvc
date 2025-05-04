using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> AuthenticateAsync(string username, string password, bool rememberMe);
        Task<bool> RegisterUserAsync(string username, string email, string password, List<int> roles);
        Task LogoutAsync();
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
