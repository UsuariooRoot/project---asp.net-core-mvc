using ECommerce.Models;
using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Reflection;

namespace ECommerce.Services.Impl
{
    public class AuthService(IUsuarioService usuarioService, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<bool> AuthenticateAsync(string username, string password, bool rememberMe)
        {
            var user = await _usuarioService.GetUserByUsernameOrEmailAsync(username);

            if (user == null)
                return false;

            if (!VerifyPassword(password, user.Pass ?? ""))
                return false;

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Username ?? "Unknown"),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var rol in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                rememberMe
                    ? new AuthenticationProperties
                    {
                        IsPersistent = true, // Para que la cookie persista más allá de la sesión del navegador
                        ExpiresUtc = DateTime.UtcNow.AddDays(7) // Expiración de la cookie
                    } : null);

            return true;
        }

        public async Task<bool> RegisterUserAsync(string username, string email, string password, List<int> roles)
        {
            string hashedPassword = HashPassword(password);

            var result = await _usuarioService.RegisterUserAsync(username, email, hashedPassword, roles);

            return !string.IsNullOrEmpty(result);
        }

        public string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
