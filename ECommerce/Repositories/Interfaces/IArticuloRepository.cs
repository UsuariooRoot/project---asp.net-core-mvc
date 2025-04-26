using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface IArticuloRepository : IRepositoryBase<Articulo>
    {
        Task<IEnumerable<Articulo>> BuscarPorAsync(int idCategoria, string? nombre, int numPagina, int tamPagina);
        Task<string> GuardarAsync(Articulo articulo);
        Task<string> EliminarAsync(int id);
    }
}
