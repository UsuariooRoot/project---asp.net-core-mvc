using Microsoft.Data.SqlClient;

namespace ECommerce.Data
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
