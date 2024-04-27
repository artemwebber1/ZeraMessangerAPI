using Microsoft.Data.SqlClient;
using SoftworkMessanger.Utilites;

namespace SoftworkMessanger.Services.Repositories
{
    public class RepositoryBase
    {
        public RepositoryBase(SqlServerConnector sqlServerConnector)
        {
            sqlServer = sqlServerConnector;
        }

        /// <summary>
        /// SQL-подключение для конкретного репозитория.
        /// </summary>
        protected readonly SqlServerConnector sqlServer;

        /// <summary>
        /// Выполняет SQL-запрос без чтения данных.
        /// </summary>
        /// <param name="sqlQuery">SQL-запрос.</param>
        protected async Task ExecuteNonQueryAsync(string sqlQuery)
        {
            using SqlConnection sqlConnection = await sqlServer.GetSqlConnectionAsync();

            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            await sqlCommand.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Выполняет SQL-запрос и проверяет, содержатся ли в нём строки.
        /// </summary>
        /// <param name="sqlQuery">SQL-запрос.</param>
        /// <returns>True, если в SQL-запросе нет строк, иначе false.</returns>
        protected async Task<bool> IsSqlQueryEmpty(string sqlQuery)
        {
            using SqlConnection sqlConnection = await sqlServer.GetSqlConnectionAsync();

            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            return sqlDataReader.HasRows;
        }

        /// <summary>
        /// Вызывает SQL-запрос и возвращает значение первого столбца первой строки.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
        /// <param name="sqlQuery">SQL-запрос.</param>
        protected async Task<T> ExecuteScalarAsync<T>(string sqlQuery)
        {
            using SqlConnection sqlConnection = await sqlServer.GetSqlConnectionAsync();
            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            T result = (T)(await sqlCommand.ExecuteScalarAsync() ?? default(T)!);

            return result;
        }
    }
}
