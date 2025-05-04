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
                await _authService.Register(username, email, pass);
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
            var user = await _authService.Login(username, pass);

            if (user == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View();
            }

            //var roles = await _usuarioService.ObtenerRolesUsuarioAsync(usuario.Id);

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, username),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var rol in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Logout", "Home");
        }
    }
}
