using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IArticuloService
    {
        Task<IEnumerable<Articulo>> ObtenerTodoAsync(int numPagina, int tamanoPagina);
        Task<IEnumerable<Articulo>> BuscarPorAsync(int idCategoria, string? nombre, int numPagina, int tamanoPagina);
        Task<Articulo> ObtenerPorIdAsync(int id);
        Task<string> AgregarArticuloAsync(Articulo articulo);
        Task<string> ActualizarArticuloAsync(Articulo articulo);
        Task<string> EliminarArticuloAsync(int id);
    }
}
