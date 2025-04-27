using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services.Impl
{
    public class ArticuloService(IArticuloRepository articuloRepository): IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository = articuloRepository;

        public async Task<Pageable<Articulo>> ObtenerTodoAsync(int numPagina, int tamanoPagina)
        {
            var articulos = await _articuloRepository.BuscarPorAsync(0, null, numPagina, tamanoPagina);
            int totalItems = await _articuloRepository.GetTotalCountAsync();

            return new Pageable<Articulo>
            {
                Items = articulos,
                CurrentPage = numPagina,
                PageSize = tamanoPagina,
                TotalItems = totalItems
            };
        }

        public async Task<Pageable<Articulo>> BuscarPorAsync(int idCategoria, string? nombre, int numPagina, int tamanoPagina)
        {
            var articulos = await _articuloRepository.BuscarPorAsync(idCategoria, nombre, numPagina, tamanoPagina);
            int totalItems = await _articuloRepository.GetTotalCountAsync(idCategoria, nombre);

            return new Pageable<Articulo>
            {
                Items = articulos,
                CurrentPage = numPagina,
                PageSize = tamanoPagina,
                TotalItems = totalItems
            };
        }

        public async Task<Articulo> ObtenerPorIdAsync(int id)
        {
            return await _articuloRepository.GetByIdAsync(id);
        }

        public async Task<string> AgregarArticuloAsync(Articulo articulo)
        {
            return await _articuloRepository.GuardarAsync(articulo);
        }

        public async Task<string> ActualizarArticuloAsync(Articulo articulo)
        {
            return await _articuloRepository.GuardarAsync(articulo);
        }

        public async Task<string> EliminarArticuloAsync(int id)
        {
            return await _articuloRepository.EliminarAsync(id);
        }
    }
}
