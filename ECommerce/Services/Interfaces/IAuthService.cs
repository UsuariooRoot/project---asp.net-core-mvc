using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(string username, string email, string pass);
        Task<Usuario> Login(string username, string pass);
    }
}
