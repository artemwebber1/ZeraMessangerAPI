using Microsoft.Data.SqlClient;
using System.Data;
using ZeraMessanger.Utilites;
using ZeraMessanger.Models;

namespace ZeraMessanger.Services.Repositories.Users
{
    public class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(SqlServerConnector sqlServerConnector) : base(sqlServerConnector) { }

        #region IUsersRepository implementation

        public async Task<User?> GetByIdAsync(int userId)
        {
            IEnumerable<User>? users = await GetUsersListFromSqlQueryAsync($@"SELECT * FROM Users WHERE Users.UserId = {userId}");
            return users?.First();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            IEnumerable<User>? users = await GetUsersListFromSqlQueryAsync(@$"SELECT * FROM Users WHERE Users.UserEmail = '{email}'");
            return users?.First();
        }

        public User GetUserFromReader(
            IDataReader dataReader,
            string userIdColumn,
            string userNameColumn,
            string userPasswordColumn,
            string userEmailColumn)
        {
            try
            {
                // Создание объекта класса User на основе данных, предоставленных читателем данных IDataReader
                User user = new User(
                    userId: (int)dataReader[userIdColumn],
                    userName: (string)dataReader[userNameColumn],
                    hashedPassword: (string)dataReader[userPasswordColumn],
                    email: (string)dataReader[userEmailColumn]);

                return user;
            }
            catch
            {
                // При ошибке чтения возвращаем null
                return null!;
            }
        }

        public async Task AddUserAsync(string name, string password, string email)
        {
            await ExecuteNonQueryAsync($@"
                INSERT INTO Users(UserName, UserPassword, UserEmail)
                VALUES('{name}', '{password}', '{email}');");
        }

        public async Task<bool> IsUserExistsWithEmail(string email)
        {
            return await IsSqlQueryEmpty($@"SELECT * FROM Users WHERE Users.UserEmail = '{email}';");
        }

        public async Task<bool> IsAdmin(int userId, int chatId)
        {
            return await IsSqlQueryEmpty($@"
                        SELECT UserChats.UserRole
                        FROM UserChats
                        WHERE
                            UserChats.UserId = {userId} AND
                            UserChats.ChatId = {chatId} AND
                            UserChats.UserRole = 'Admin';");
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
                User? user = GetUserFromReader(
                    dataReader,
                    userIdColumn: "UserId",
                    userNameColumn: "UserName",
                    userPasswordColumn: "UserPassword",
                    userEmailColumn: "UserEmail");
                if (user != null)
                    users.AddLast(user);
            }

            return users;
        }
    }
}
