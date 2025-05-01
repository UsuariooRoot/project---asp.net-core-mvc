using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Repositories.Impl
{
    public class CarritoRepository(IDbConnectionFactory connectionFactory) : BaseRepository(connectionFactory), ICarritoRepository
    {

        public async Task<int> RegistrarVentaAsync(int idUsuario, decimal total, string metodoEntrega)
        {
            int idventa = 0;
            using (SqlConnection cn = CreateConnection())
            {
                await cn.OpenAsync();
                SqlCommand cmdVenta = new ("usp_registrar_venta", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmdVenta.Parameters.AddWithValue("@id_usuario", idUsuario);
                cmdVenta.Parameters.AddWithValue("@total", total);
                cmdVenta.Parameters.AddWithValue("@metodo_entrega", metodoEntrega);
                SqlParameter idOut = new ("@id_venta", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmdVenta.Parameters.Add(idOut);
                await cmdVenta.ExecuteNonQueryAsync();
                idventa = (int)idOut.Value;
            }
            return idventa;
        }

        public async Task RegistrarDetalleVentaAsync(int idVenta, int idArticulo, int cantidad, decimal precioUnitario)
        {
            using SqlConnection cn = CreateConnection();
            await cn.OpenAsync();
            SqlCommand cmd = new("usp_registrar_detalle_venta", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id_venta", idVenta);
            cmd.Parameters.AddWithValue("@id_articulo", idArticulo);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.Parameters.AddWithValue("@precio_unitario", precioUnitario);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
