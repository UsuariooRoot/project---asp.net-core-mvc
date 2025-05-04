using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsersAsync();
        Task<Usuario?> GetUserByUsernameOrEmailAsync(string input);
        Task<string> RegisterUserAsync(string username, string email, string pass, List<int> roles);
        Task<string> EliminarUsuarioAsync(int id);
    }
}