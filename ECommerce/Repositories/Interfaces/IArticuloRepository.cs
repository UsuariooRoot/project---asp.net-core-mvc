using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface IArticuloRepository : IRepositoryBase<Articulo>
    {
        Task<IEnumerable<Articulo>> BuscarPorAsync(int idcategoria, string? nombre = null, int numPagina = 1, int tamPagina = 10);
        Task<int> GetTotalCountAsync(int idcategoria = 0, string? nombre = null);
        Task<string> GuardarAsync(Articulo articulo);
        Task<string> EliminarAsync(int id);
    }
}
