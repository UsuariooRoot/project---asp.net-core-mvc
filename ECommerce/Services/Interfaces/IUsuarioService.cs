using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> BuscarUsuarioPorAsync(int? idUsuario = null, string? username = null, string? email = null);
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        Task<string> RegistrarUsuarioAsync(string username, string email, string pass, List<int> roles);
        Task<List<string>> ObtenerRolesUsuarioAsync(int idUsuario);
        Task<string> EliminarUsuarioAsync(int id);
    }
}