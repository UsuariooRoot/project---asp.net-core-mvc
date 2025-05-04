using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services.Impl
{
    public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<IEnumerable<Usuario>> GetAllUsersAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetUserByUsernameOrEmailAsync(string input)
        {   
            if (input != null)
            {
                return await _usuarioRepository.GetByUsernameAsync(input);
            }
            
            return null;
        }

        public async Task<string> RegisterUserAsync(string username, string email, string pass, List<int> roles)
        {
            if (roles == null || roles.Count > 0)
            {
                // diplock -> establecer un rol predeterminado. Un usuario no puede registrarse sin un rol
                roles = [1];
            }

            return await _usuarioRepository.SaveAsync(username, email, pass, roles);
        }

        public async Task<string> EliminarUsuarioAsync(int id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }
    }
}
