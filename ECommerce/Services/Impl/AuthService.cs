using ECommerce.Models;
using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;

namespace ECommerce.Services.Impl
{
    public class AuthService(IUsuarioService usuarioService) : IAuthService
    {
        private readonly IUsuarioService _usuarioService = usuarioService;

        public async Task<Usuario?> AuthenticateAsync(string username, string password)
        {
            var user = await _usuarioService.GetUserByUsernameOrEmailAsync(username);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.Pass ?? ""))
                return null;

            return user;
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
    }
}
