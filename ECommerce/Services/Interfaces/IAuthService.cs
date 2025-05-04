using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario?> AuthenticateAsync(string username, string password);
        Task<bool> RegisterUserAsync(string username, string email, string password, List<int> roles);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
