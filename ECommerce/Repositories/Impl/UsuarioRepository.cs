using Microsoft.Data.SqlClient;
using ECommerce.Models;
using System.Data;
using ECommerce.Repositories.Interfaces;
using ECommerce.Data;

namespace ECommerce.Repositories.Impl
{
    public class UsuarioRepository(IDbConnectionFactory connectionFactory) : BaseRepository(connectionFactory), IUsuarioRepository
    {

        private static Usuario DataReaderToUsuario(SqlDataReader dr)
        {

            return new Usuario
            {
                Id = dr.GetInt32(0),
                Username = dr.GetString(1),
                Email = dr.GetString(2),
                Pass = dr.GetString(3),
                FechaCreacion = dr.GetDateTime(4)
            };
        }

        private static DataTable CrearDataTableRoles(List<int> roles)
        {
            var table = new DataTable();
            table.Columns.Add("id_rol", typeof(int));
            foreach (var rol in roles)
            {
                table.Rows.Add(rol);
            }
            return table;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            List<Usuario> temporal = [];
            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();
                SqlCommand cmd = new("usp_listar_usuarios", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    temporal.Add(DataReaderToUsuario(dr));
                }
                await dr.CloseAsync();
            }
            return temporal;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            using var cn = CreateConnection();

            await cn.OpenAsync();
            SqlCommand cmd = new("usp_buscar_usuario", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@id_usuario", id);

            SqlDataReader dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return DataReaderToUsuario(dr);
            }

            return null;
        }

        public async Task<Usuario> BuscarPorAsync(string? username = null, string? email = null)
        {
            using var cn = CreateConnection();
            await cn.OpenAsync();
            using var cmd = new SqlCommand("usp_buscar_usuario", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (username != null)
            {
                cmd.Parameters.AddWithValue("@username", username);
            }

            if (email != null)
            {
                cmd.Parameters.AddWithValue("@email", email);
            }

            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return DataReaderToUsuario(dr);
            }

            return null;
        }

        public async Task<string> RegistrarAsync(string username, string email, string pass, List<int> roles)
        {
            using var cn = CreateConnection();
            try
            {
                await cn.OpenAsync();
                using var cmd = new SqlCommand("usp_registrar_usuario", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", pass);
                var tvpParam = cmd.Parameters.AddWithValue("@roles", CrearDataTableRoles(roles));
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "TipoRoles";

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return $"Se ha registrado {rowsAffected} usuario";
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al registrar usuario: {ex.Message}", ex);
            }
        }

        public async Task<List<string>> ObtenerRolesUsuarioAsync(int id)
        {
            var roles = new List<string>();

            using (var cn = CreateConnection())
            {
                await cn.OpenAsync();
                using var cmd = new SqlCommand("usp_obtener_roles_usuario", (SqlConnection)cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", id);

                using var dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    roles.Add(dr.GetString(0));
                }
            }

            return roles;
        }

        public async Task<string> EliminarAsync(int id)
        {
            using var cn = CreateConnection();
            await cn.OpenAsync();
            using var cmd = new SqlCommand("usp_eliminar_usuario", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@id_usuario", id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return $"Se ha eliminado {rowsAffected} usuario";
        }
    }
}
