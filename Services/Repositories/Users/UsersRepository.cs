using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
using SoftworkMessanger.Utilites;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Users
{
    public class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(SqlServerConnector sqlServerConnector) : base(sqlServerConnector) { }

        #region IUsersRepository implementation

        public User? GetById(int userId)
        {
            User? user = GetUsersListFromSqlQueryAsync($@"SELECT * FROM Users WHERE Users.UserId = {userId}").Result?.First();
            return user;
        }

        public User? GetByUserName(string userName)
        {
            User? user = GetUsersListFromSqlQueryAsync(@$"SELECT * FROM Users WHERE Users.UserName = '{userName}'").Result?.First();
            return user;
        }

        public User GetUserFromReader(IDataReader dataReader)
        {
            try
            {
                User user = new User
                {
                    UserId = (int)dataReader["UserID"],
                    UserName = (string)dataReader["UserName"],
                    UserHashedPassword = (string)dataReader["UserPassword"],
                    UserEmail = (string)dataReader["UserEmail"]
                };

                return user;
            }
            catch
            {
                // При ошибке чтения возвращаем null
                return null!;
            }
        }

        #endregion

        
        /// <summary>
        /// Получение списка пользователей из SQL-запроса.
        /// </summary>
        /// <param name="sqlQuery">SQL-запрос.</param>
        /// <returns>Список пользователей, полученный из SQL-запроса.</returns>
        private async Task<IEnumerable<User>?> GetUsersListFromSqlQueryAsync(string sqlQuery)
        {
            using SqlConnection sqlConnection = await sqlServer.GetSqlConnectionAsync();

            // Создание команды на основе SQL-запроса
            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            // Создание читателя данных SQL-запроса
            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
            
            // Если у SQL-запроса нет строк, возвращаем null
            if (!reader.HasRows)
                return null;

            // Получение списка пользователей из читателя данных SQL-запроса
            IEnumerable<User> users = await GetUsersListFromReaderAsync(reader);
            return users;
        }

        /// <summary>
        /// Получение списка пользователей из читателя данных SQL-запроса <paramref name="dataReader"/>.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Список пользователей, полученный из читателя данных SQL-запроса.</returns>
        private async Task<IEnumerable<User>> GetUsersListFromReaderAsync(SqlDataReader dataReader)
        {
            // Здесь в качестве структуры данных
            // используется связанный список для быстрой вставки нового пользователя в результирующий список
            LinkedList<User> users = new LinkedList<User>();

            while (await dataReader.ReadAsync())
            {
                User? user = GetUserFromReader(dataReader);
                if (user != null)
                    users.AddLast(user);
            }

            return users;
        }
    }
}
