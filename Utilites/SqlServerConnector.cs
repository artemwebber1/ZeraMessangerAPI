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

        public SqlConnection OpenedSqlConnection
        {
            get
            {
                SqlConnection sqlConnection = new SqlConnection(_connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
        }

        public async Task<SqlConnection> GetSqlConnectionAsync()
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}
