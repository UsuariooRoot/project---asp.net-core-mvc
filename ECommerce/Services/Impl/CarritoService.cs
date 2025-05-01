using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services.Impl
{
    public class CarritoService(ICarritoRepository carritoRepository, IArticuloRepository articuloRepository) : ICarritoService
    {
        private const string SessionKey = "Carrito";
        private readonly ICarritoRepository _carritoRepository = carritoRepository;
        private readonly IArticuloRepository _articuloRepository = articuloRepository;

        public List<Carrito> ObtenerCarrito(HttpContext httpContext)
        {
            var carrito = httpContext.Session.GetObjectFromJson<List<Carrito>>(SessionKey);
            return carrito ?? [];
        }

        public void GuardarCarrito(HttpContext httpContext, List<Carrito> carrito)
        {
            httpContext.Session.SetObjectAsJson(SessionKey, carrito);
        }

        public async Task<bool> AgregarAlCarritoAsync(HttpContext httpContext, int articuloId)
        {
            List<Carrito> carrito = ObtenerCarrito(httpContext);
            var articulo = await _articuloRepository.GetByIdAsync(articuloId);

            if (articulo == null) return false;

            var existente = carrito.FirstOrDefault(c => c.IdArticulo == articulo.IdArticulo);
            if (existente != null)
                existente.Cantidad++;
            else
            {
                carrito.Add(new Carrito
                {
                    IdArticulo = articulo.IdArticulo,
                    Nombre = articulo.Nombre,
                    Descripcion = articulo.Descripcion,
                    Precio = articulo.Precio,
                    Cantidad = 1
                });
            }

            GuardarCarrito(httpContext, carrito);
            return true;
        }

        public async Task<int> ConfirmarCompraAsync(HttpContext httpContext, int idUsuario, string metodoEntrega)
        {
            var carrito = ObtenerCarrito(httpContext);
            if (!carrito.Any()) return 0;

            decimal total = carrito.Sum(c => c.Subtotal);
            int idVenta = await _carritoRepository.RegistrarVentaAsync(idUsuario, total, metodoEntrega);

            foreach (var item in carrito)
            {
                await _carritoRepository.RegistrarDetalleVentaAsync(idVenta, item.IdArticulo, item.Cantidad, item.Precio);
            }

            httpContext.Session.Remove(SessionKey);
            return idVenta;
        }
    }
}