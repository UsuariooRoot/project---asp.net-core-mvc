using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Repositories.Impl
{
    public class ArticuloRepository(IDbConnectionFactory connectionFactory) : BaseRepository(connectionFactory), IArticuloRepository
    {
        private static Articulo DataReaderToArticulo(SqlDataReader dr)
        {
            return new Articulo()
            {
                IdArticulo = dr.GetInt32(0),
                IdCategoria = dr.GetInt32(1),
                Nombre = dr.GetString(2),
                Precio = dr.GetDecimal(3),
                Stock = dr.GetInt32(4),
                Descripcion = dr.GetString(5),

            };
        }

        public async Task<IEnumerable<Articulo>> GetAllAsync()
        {
            List<Articulo> temporal = [];
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();
                SqlCommand cmd = new("usp_listar_articulos", cn);
                SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    temporal.Add(DataReaderToArticulo(dr));
                }
                await dr.CloseAsync();
            }
            return temporal;
        }

        public async Task<Articulo> GetByIdAsync(int id)
        {
            List<Articulo> temporal = [];
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();
                SqlCommand cmd = new("usp_buscar_articulo_por_id", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    temporal.Add(DataReaderToArticulo(dr));
                }
                await dr.CloseAsync();
            }
            return temporal[0];
        }

        public async Task<int> GetTotalCountAsync(int idcategoria = 0, string? nombre = null)
        {
            int count = 0;
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();
                SqlCommand cmd = new("usp_contar_articulos", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (idcategoria > 0)
                {
                    cmd.Parameters.AddWithValue("@id_categoria", idcategoria);
                }
                if (!string.IsNullOrEmpty(nombre))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                }
                count = (int)await cmd.ExecuteScalarAsync();
            }
            return count;
        }

        public async Task<IEnumerable<Articulo>> BuscarPorAsync(int idcategoria = 0, string? nombre = null, int numPagina = 1, int tamPagina = 10)
        {
            List<Articulo> temporal = [];
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();

                SqlCommand cmd = new("usp_listar_articulos", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (idcategoria > 0)
                {
                    cmd.Parameters.AddWithValue("@id_categoria", idcategoria);
                }

                if (!string.IsNullOrEmpty(nombre))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                }

                cmd.Parameters.AddWithValue("@num_pagina", numPagina);
                cmd.Parameters.AddWithValue("@tam_pagina", tamPagina);

                SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    temporal.Add(DataReaderToArticulo(dr));
                }
                await dr.CloseAsync();
            }
            return temporal;
        }

        public async Task<string> GuardarAsync(Articulo articulo)
        {
            string mensaje = "";
            using (var cn = CreateConnection())
            {
                try
                {
                    await cn.OpenAsync();
                    string procedimiento = articulo.IdArticulo > 0 ? "usp_modificar_articulo" : "usp_agregar_articulo";

                    SqlCommand cmd = new(procedimiento, cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    if (articulo.IdArticulo > 0)
                        cmd.Parameters.AddWithValue("@id_articulo", articulo.IdArticulo);

                    cmd.Parameters.AddWithValue("@id_categoria", articulo.IdCategoria);
                    cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                    cmd.Parameters.AddWithValue("@stock", articulo.Stock);
                    cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);

                    int i = await cmd.ExecuteNonQueryAsync();
                    mensaje = articulo.IdArticulo > 0
                        ? $"Se ha actualizado {i} articulo"
                        : $"Se ha insertado {i} articulo";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        public async Task<string> EliminarAsync(int id)
        {
            string mensaje = "";
            using (var cn = CreateConnection())
            {
                try
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new("usp_eliminar_articulo", cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@id_articulo", id);

                    int i = await cmd.ExecuteNonQueryAsync();
                    mensaje = $"Se ha eliminado {i} articulo";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
            }
            return mensaje;
        }

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
