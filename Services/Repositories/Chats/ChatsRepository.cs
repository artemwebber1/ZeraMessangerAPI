using Microsoft.Data.SqlClient;
using SoftworkMessanger.Extensions;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto;
using SoftworkMessanger.Services.Repositories.Messages;
using SoftworkMessanger.Services.Repositories.Users;
using SoftworkMessanger.Utilites;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Chats
{
    public class ChatsRepository : IChatsRepository
    {
        public ChatsRepository(SqlServerConnector sqlServerConnector, IUsersRepository usersRepository, IMessagesRepository messagesRepository)
        {
            _sqlServerConnector = sqlServerConnector;
            _usersRepository = usersRepository;
            _messagesRepository = messagesRepository;
        }

        /// <summary>
        /// Провайдер базы данных MS SQL Server.
        /// </summary>
        private readonly SqlServerConnector _sqlServerConnector;

        #region Repositories

        /// <summary>
        /// Репозиторий для работы с таблицей пользователей.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// Репозиторий для работы с таблицей сообщений.
        /// </summary>
        private readonly IMessagesRepository _messagesRepository;

        #endregion

        public IEnumerable<Chat>? GetById(int chatId)
        {
            return GetChatsListFromSqlQuery(
                $"SELECT * FROM Chats LEFT JOIN Messages ON Messages.ChatId = Chats.ChatId WHERE Chats.ChatId = {chatId};")
                .Result;
        }

        public IEnumerable<ChatFirstView>? GetUserChats(int userId)
        {
            IEnumerable<Chat>? chats = GetChatsListFromSqlQuery(
                $"SELECT * " +
                $"FROM UserChats " +
                $"LEFT JOIN Chats ON Chats.ChatId = UserChats.ChatId " +
                $"LEFT JOIN Messages ON Messages.ChatId = UserChats.ChatId " +
                $"WHERE UserChats.UserId = {userId};")
                .Result;

            // Конвертация обычных моделей чатов в DTO-модели
            List<ChatFirstView>? chatFirstViews = new List<ChatFirstView>();
            if (chats != null)
            {
                foreach (Chat chat in chats)
                    chatFirstViews.Add(chat.ToChatFirstView());
            }

            return chatFirstViews;
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
        private async Task<IEnumerable<Chat>> GetChatsListFromReader(SqlDataReader dataReader)
        {
            List<Chat> chats = new List<Chat>();

            while (await dataReader.ReadAsync())
            {
                int chatId = (int)dataReader["ChatId"];

                // Получаем чат из читателя данных и добавляем к нему построчно сообщения, которые в нём находятся
                Chat? chat = chats.Find(c => c.ChatId == chatId)?.IncludeChatMessages(dataReader, _messagesRepository);
                // Если чат с таким id попался впервые, создаём новый объект
                if (chat == null)
                {
                    chat = GetChatFromReader(dataReader)?.IncludeChatMessages(dataReader, _messagesRepository);
                    if (chat != null)
                        chats.Add(chat);
                }
            }

            return chats;
        }

        public Chat GetChatFromReader(SqlDataReader dataReader)
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
                return null!;
            }
        }
    }
}
