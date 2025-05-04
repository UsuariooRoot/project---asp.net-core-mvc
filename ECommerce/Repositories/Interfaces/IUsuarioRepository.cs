using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<string> SaveAsync(string username, string email, string pass, List<int> roles);
        Task<string> DeleteAsync(int id);
    }
}
