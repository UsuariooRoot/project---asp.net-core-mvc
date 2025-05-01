using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface ICarritoService
    {
        List<Carrito> ObtenerCarrito(HttpContext httpContext);
        void GuardarCarrito(HttpContext httpContext, List<Carrito> carrito);
        Task<bool> AgregarAlCarritoAsync(HttpContext httpContext, int articuloId);
        Task<int> ConfirmarCompraAsync(HttpContext httpContext, int idUsuario, string metodoEntrega);
    }
}
