namespace ECommerce.Repositories.Interfaces
{
    public interface ICarritoRepository
    {
        Task<int> RegistrarVentaAsync(int idUsuario, decimal total, string metodoEntrega);
        Task RegistrarDetalleVentaAsync(int idVenta, int idArticulo, int cantidad, decimal precioUnitario);
    }
}
