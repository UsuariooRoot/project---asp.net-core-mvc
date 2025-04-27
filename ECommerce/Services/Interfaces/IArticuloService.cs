using ECommerce.Models;
using ECommerce.Helpers;

namespace ECommerce.Services.Interfaces
{
    public interface IArticuloService
    {
        Task<Pageable<Articulo>> ObtenerTodoAsync(int numPagina, int tamanoPagina);
        Task<Pageable<Articulo>> BuscarPorAsync(int idCategoria, string? nombre, int numPagina, int tamanoPagina);
        Task<Articulo> ObtenerPorIdAsync(int id);
        Task<string> AgregarArticuloAsync(Articulo articulo);
        Task<string> ActualizarArticuloAsync(Articulo articulo);
        Task<string> EliminarArticuloAsync(int id);
    }
}
