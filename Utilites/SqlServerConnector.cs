using Microsoft.Data.SqlClient;

namespace SoftworkMessanger.Utilites
{
    public class SqlServerConnector
    {
        public SqlServerConnector(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:SqlServerConnectionString"];
        }

        private readonly string? _connectionString;

        public async Task<SqlConnection> GetSqlConnectionAsync()
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}
