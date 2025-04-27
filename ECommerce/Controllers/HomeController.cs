using Microsoft.AspNetCore.Mvc;
using ECommerce.Services.Interfaces;
using ECommerce.Models;
using ECommerce.Helpers;

namespace ECommerce.Controllers
{
    public class HomeController(IArticuloService articuloService) : Controller
    {
        private readonly IArticuloService _articuloService = articuloService;

        public async Task<IActionResult> Index(string? nombre, int pagina = 1, int size = 10)
        {
            ViewBag.Nombre = nombre;
            ViewBag.Size = size;

            Pageable<Articulo> resultado;

            if (nombre != null)
            {
                resultado = await _articuloService.BuscarPorAsync(0, nombre, pagina, size);
            }
            else
            {
                resultado = await _articuloService.ObtenerTodoAsync(pagina, size);
            }

            return View(resultado);
        }
    }
}