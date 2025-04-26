using Microsoft.AspNetCore.Mvc;
using ECommerce.Services.Interfaces;

namespace ECommerce.Controllers
{
    public class HomeController(IArticuloService articuloService) : Controller
    {
        private readonly IArticuloService _articuloService = articuloService;
        //private readonly IUsuarioService _usuarioService = usuarioService;

        public async Task<IActionResult> Index(int? categoria, string? nombre, int? pagina, int? size)
        {
            Console.WriteLine("pagina: " + pagina + " size: " + size);
            pagina ??= 1;
            size ??= 10;

            ViewBag.p = pagina;

            if (nombre != null || categoria != null)
            {
                categoria ??= 0;
                return View(await _articuloService.BuscarPorAsync((int)categoria, nombre, (int)pagina, (int)size));
            }

            var articulos = await _articuloService.ObtenerTodoAsync((int)pagina, (int)size);


            return View(articulos.ToList());
        }
    }
}