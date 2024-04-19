using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
using SoftworkMessanger.Utilites;

namespace SoftworkMessanger.Services.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        public UsersRepository(SqlServerConnector sqlServerConnector)
        {
            _sqlServerConnector = sqlServerConnector;
        }

        private readonly SqlServerConnector _sqlServerConnector;

        public User GetById(int userId)
        {
            User user = GetUserCollectionFromSqlQueryAsync(@$"SELECT * FROM Users WHERE Users.UserId = {userId}").Result.First();
            return user;
        }

        public User GetByUserName(string userName)
        {
            User user = GetUserCollectionFromSqlQueryAsync(@$"SELECT * FROM Users WHERE Users.UserName = '{userName}'").Result.First();
            return user;
        }

        private async Task<IEnumerable<User>> GetUserCollectionFromSqlQueryAsync(string sqlQuery)
        {
            using SqlConnection sqlConnection = await _sqlServerConnector.GetSqlConnectionAsync();

            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
            if (!reader.HasRows)
                return null;

            IEnumerable<User> users = await GetUsersFromReaderAsync(reader);
            return users;
        }

        private async Task<IEnumerable<User>> GetUsersFromReaderAsync(SqlDataReader dataReader)
        {
            LinkedList<User> users = new LinkedList<User>();

            while (await dataReader.ReadAsync())
            {
                User? user = GetUserFromReader(dataReader);
                if (user != null)
                    users.AddLast(user);
            }

            return users;
        }

        private User? GetUserFromReader(SqlDataReader dataReader)
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
                return null;
            }
        }
    }
}
