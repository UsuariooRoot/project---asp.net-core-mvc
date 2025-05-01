using Microsoft.Data.SqlClient;

namespace ECommerce.Data
{
    public class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
    {
        private readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"]
                ?? throw new ArgumentNullException("ConnectionString no encontrado");

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
