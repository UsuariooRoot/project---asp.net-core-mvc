using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Repositories.Impl
{
    public class CategoriaRepository(IDbConnectionFactory connectionFactory) : BaseRepository(connectionFactory), ICategoriaRepository

    {
        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            List<Categoria> categorias = [];
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();
                SqlCommand cmd = new("usp_listar_categorias", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    categorias.Add(new Categoria
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1)
                    });
                }
                await dr.CloseAsync();
            }
            return categorias;
        }
    }
}
