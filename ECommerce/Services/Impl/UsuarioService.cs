using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services.Impl
{
    public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario> BuscarUsuarioPorAsync(int? idUsuario = null, string? username = null, string? email = null)
        {
            if (idUsuario != null)
            {
                return await _usuarioRepository.GetByIdAsync((int)idUsuario);
            }
            return await _usuarioRepository.BuscarPorAsync(username, email);
        }

        public async Task<string> RegistrarUsuarioAsync(string username, string email, string pass, List<int> roles)
        {
            if (roles == null || roles.Count > 0)
            {
                // diplock -> establecer un rol predeterminado. Un usuario no puede registrarse sin un rol
                roles = [1];
            }

            return await _usuarioRepository.RegistrarAsync(username, email, pass, roles);
        }

        public async Task<List<string>> ObtenerRolesUsuarioAsync(int idUsuario)
        {
            return await _usuarioRepository.ObtenerRolesUsuarioAsync(idUsuario);
        }

        public async Task<string> EliminarUsuarioAsync(int id)
        {
            return await _usuarioRepository.EliminarAsync(id);
        }
    }
}
