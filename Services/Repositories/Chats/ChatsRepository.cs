using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
using SoftworkMessanger.Utilites;

namespace SoftworkMessanger.Services.Repositories.Chats
{
    public class ChatsRepository : IChatsRepository
    {
        public ChatsRepository(SqlServerConnector sqlServerConnector)
        {
            _sqlServerConnector = sqlServerConnector;
        }

        /// <summary>
        /// Провайдер базы данных MS SQL Server.
        /// </summary>
        private readonly SqlServerConnector _sqlServerConnector;

        public Chat? GetById(int chatId)
        {
            Chat? chat = GetChatsListFromSqlQuery($"SELECT * FROM Chats WHERE Chats.ChatId = {chatId}").Result?.First();
            return chat;
        }

        public IEnumerable<Chat>? GetUserChats(int userId)
        {
            IEnumerable<Chat>? chats = GetChatsListFromSqlQuery(
                "SELECT *" +
                " FROM Chats LEFT JOIN Users ON Users.UserId = Chats.ChatId" +
                $" WHERE Users.UserId = {userId};").Result;

            return chats;
        }

        /// <summary>
        /// Получение набора чатов из SQL-запроса.
        /// </summary>
        /// <param name="sqlQuery">SQL-запрос.</param>
        /// <returns>Набор чатов, полученный из указанного SQL-запроса (<paramref name="sqlQuery"/>).</returns>
        private async Task<IEnumerable<Chat>?> GetChatsListFromSqlQuery(string sqlQuery)
        {
            // Установлене подключения к базе данных
            using SqlConnection sqlConnection = await _sqlServerConnector.GetSqlConnectionAsync();

            // Создание команды на основе SQL-запроса
            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            // Создание читателя данных из SQL-команды
            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            if (!sqlDataReader.HasRows)
                return null;

            // Получение набора чатов из читателя данных SQL-запроса.
            IEnumerable<Chat> chats = await GetChatsListFromReader(sqlDataReader);
            return chats;
        }

        /// <summary>
        /// Получение набора чатов из читателя данных SQL-запроса.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Набор чатов из читателя данных SQL-запроса.</returns>
        private static async Task<IEnumerable<Chat>> GetChatsListFromReader(SqlDataReader dataReader)
        {
            // Связанный список нужен для быстрого добавления нового элемента в результирующий список.
            LinkedList<Chat> chats = new LinkedList<Chat>();

            while (await dataReader.ReadAsync())
            {
                Chat? chat = GetChatFromReader(dataReader);
                if (chat != null)
                    chats.AddLast(chat);
            }

            return chats;
        }

        /// <summary>
        /// Получение конкретного чата из читателя данных SQL-запроса.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Объект класса <see cref="Chat"/>, прочитанный из читателя данных SQL-запроса.</returns>
        private static Chat? GetChatFromReader(SqlDataReader dataReader)
        {
            try
            {
                Chat chat = new Chat
                {
                    ChatId = (int)dataReader["ChatId"],
                    ChatName = (string)dataReader["ChatName"]
                };

                return chat;
            }
            catch
            {
                return null;
            }
        }
    }
}
