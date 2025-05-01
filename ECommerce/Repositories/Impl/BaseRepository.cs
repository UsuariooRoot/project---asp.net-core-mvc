using Microsoft.Data.SqlClient;
using ECommerce.Data;

namespace ECommerce.Repositories.Impl
{
    public abstract class BaseRepository(IDbConnectionFactory connectionFactory)
    {
        protected readonly IDbConnectionFactory _connectionFactory = connectionFactory;

        protected SqlConnection CreateConnection()
        {
            return _connectionFactory.CreateConnection();
        }
    }
}
