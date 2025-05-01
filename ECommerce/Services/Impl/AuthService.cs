using ECommerce.Models;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services.Impl
{
    public class AuthService(IUsuarioService usuarioService) : IAuthService
    {
        private readonly IUsuarioService _usuarioService = usuarioService;

        public async Task<Usuario> Login(string username, string pass)
        {
            var usuario = await _usuarioService.BuscarUsuarioPorAsync(null, username, username);

            // diplock -> Usar BCrypt.Verify en producción
            if (usuario != null && pass != usuario.Pass)
                return null;

            return usuario;
        }

        public async Task<string> Register(string username, string email, string pass)
        {
            List<int> roles = [1]; // diplock -> establecer un rol predeterminado. Un usuario no puede registrarse sin un rol
            return await _usuarioService.RegistrarUsuarioAsync(username, email, pass, roles);
        }
    }
}
