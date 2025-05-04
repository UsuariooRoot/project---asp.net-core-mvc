using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerce.Services.Interfaces;
using ECommerce.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controllers
{
    public class AccountController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        public IActionResult Index() => RedirectToAction("Login");

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<int> roles = [ 2 ]; // USER role

                var result = await _authService.RegisterUserAsync(
                    model.Username,
                    model.Email,
                    model.Password,
                    roles);

                if (result)
                {
                    // diplock -> auto-login
                    await _authService.AuthenticateAsync(model.Username, model.Password, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Error al registrar el usuario.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.AuthenticateAsync(
                model.Username,
                model.Password,
                model.RememberMe);

            if (!result)
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
                return RedirectToAction("Login");
            }
                
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
