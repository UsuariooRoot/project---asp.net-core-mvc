using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerce.Services.Interfaces;

namespace ECommerce.Controllers
{
    public class AuthController(IAuthService authService, IUsuarioService usuarioService) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly IUsuarioService _usuarioService= usuarioService;

        public IActionResult Index() => RedirectToAction("Login");

        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(string username, string email, string pass)
        {
            try
            {
                await _authService.RegisterUserAsync(username, email, pass, [1]);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string pass)
        {
            var user = await _authService.AuthenticateAsync(username, pass);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
                return RedirectToAction("Login");
            }

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
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true, // Para que la cookie persista más allá de la sesión del navegador
                    ExpiresUtc = DateTime.UtcNow.AddDays(7) // Expiración de la cookie
                });

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
