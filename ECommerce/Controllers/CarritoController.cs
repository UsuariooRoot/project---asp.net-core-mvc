using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    public class CarritoController(ICarritoService carritoService) : Controller
    {
        private readonly ICarritoService _carritoService = carritoService;

        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int articuloId)
        {
            await _carritoService.AgregarAlCarritoAsync(HttpContext, articuloId);
            return RedirectToAction("VerCarrito");
        }

        public IActionResult VerCarrito()
        {
            var carrito = _carritoService.ObtenerCarrito(HttpContext);
            ViewBag.Total = carrito.Sum(c => c.Subtotal);
            return View(carrito);
        }

        public IActionResult Aumentar(int id)
        {
            var carrito = _carritoService.ObtenerCarrito(HttpContext);
            var item = carrito.FirstOrDefault(x => x.IdArticulo == id);
            if (item != null)
                item.Cantidad++;
            _carritoService.GuardarCarrito(HttpContext, carrito);
            return RedirectToAction("VerCarrito");
        }

        public IActionResult Disminuir(int id)
        {
            var carrito = _carritoService.ObtenerCarrito(HttpContext);
            var item = carrito.FirstOrDefault(x => x.IdArticulo == id);
            if (item != null && item.Cantidad > 1)
                item.Cantidad--;
            else if (item != null)
                carrito.Remove(item);

            _carritoService.GuardarCarrito(HttpContext, carrito);
            return RedirectToAction("VerCarrito");
        }

        public IActionResult Eliminar(int id)
        {
            var carrito = _carritoService.ObtenerCarrito(HttpContext);
            carrito.RemoveAll(x => x.IdArticulo == id);
            _carritoService.GuardarCarrito(HttpContext, carrito);
            return RedirectToAction("VerCarrito");
        }

        [HttpGet]
        public IActionResult ConfirmarCompra()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToRoute("");
                //return RedirectToAction("Login", "Negocios");

            var carrito = _carritoService.ObtenerCarrito(HttpContext);
            if (!carrito.Any()) return RedirectToAction("VerCarrito");

            return View(carrito);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarCompra(string metodoEntrega)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToRoute("/");
                // return RedirectToAction("Index", "Home");

            var carrito = _carritoService.ObtenerCarrito(HttpContext);
            if (!carrito.Any()) return RedirectToAction("VerCarrito");

            int idusuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            decimal total = carrito.Sum(c => c.Subtotal);

            int idventa = await _carritoService.ConfirmarCompraAsync(HttpContext, idusuario, metodoEntrega);

            ViewBag.Mensaje = $"✅ ¡Compra registrada con éxito! ID Venta: {idventa}\n" +
                              $"Método de entrega: {metodoEntrega}\n" +
                              $"Total: {total:C}";

            return View("CompraExitosa");
        }
    }
}
