using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services.Impl
{
    public class ArticuloService(IArticuloRepository articuloRepository): IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository = articuloRepository;

        public Task<IEnumerable<Articulo>> ObtenerTodoAsync(int numPagina, int tamanoPagina)
        {
            return _articuloRepository.BuscarPorAsync(0, null, numPagina, tamanoPagina);
        }

        public Task<IEnumerable<Articulo>> BuscarPorAsync(int idCategoria, string? nombre, int numPagina, int tamanoPagina)
        {
            return _articuloRepository.BuscarPorAsync(idCategoria, nombre, numPagina, tamanoPagina);
        }

        public Task<string> ActualizarArticuloAsync(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public Task<string> AgregarArticuloAsync(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public Task<string> EliminarArticuloAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Articulo> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
