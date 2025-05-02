using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<string> RegistrarAsync(string username, string email, string pass, List<int> roles);
        Task<string> EliminarAsync(int id);
        Task<List<string>> ObtenerRolesUsuarioAsync(int idUsuario);
    }
}
