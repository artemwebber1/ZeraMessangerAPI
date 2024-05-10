﻿using Microsoft.Data.SqlClient;
using SoftworkMessanger.Extensions;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.ChatDto;
using SoftworkMessanger.Services.Repositories.Messages;
using SoftworkMessanger.Utilites;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Chats
{
    public class ChatsRepository : RepositoryBase, IChatsRepository
    {
        public ChatsRepository(SqlServerConnector sqlServerConnector, IMessagesRepository messagesRepository)
            : base(sqlServerConnector)
        {
            _messagesRepository = messagesRepository;
        }

        /// <summary>
        /// Репозиторий для работы с таблицей сообщений.
        /// </summary>
        private readonly IMessagesRepository _messagesRepository;

        #region IChatsRepository implementation

        public async Task<Chat?> GetByIdAsync(int chatId)
        {
            IEnumerable<Chat>? chats = await GetChatsListFromSqlQuery(
                $@" SELECT 
	                    Chats.ChatId, 
	                    Chats.ChatName, 
	                    Messages.MessageId,
	                    Messages.AuthorId,
                        Messages.MessageText,
                        Messages.ChatId,
	                    Users.UserName
                    FROM Chats
                    LEFT JOIN Messages ON Messages.ChatId = Chats.ChatId
                    LEFT JOIN Users ON Messages.AuthorId = Users.UserId
                    WHERE Chats.ChatId = {chatId};");

            return chats?.First();
        }

        public async Task<IEnumerable<ChatFirstView>> GetUserChatsAsync(int userId)
        {
            IEnumerable<Chat>? chats = await GetChatsListFromSqlQuery(
                $@"SELECT * 
                   FROM UserChats 
                   LEFT JOIN Chats ON Chats.ChatId = UserChats.ChatId 
                   LEFT JOIN Messages ON Messages.ChatId = UserChats.ChatId
                   WHERE UserChats.UserId = {userId};");

            // Конвертация моделей чатов в DTO-модели
            List<ChatFirstView>? chatFirstViews = new List<ChatFirstView>();
            if (chats != null)
            {
                foreach (Chat chat in chats)
                    chatFirstViews.Add(chat.ToChatFirstView());
            }

            return chatFirstViews;
        }

        public async Task<Chat?> GetChatFromReader(IDataReader dataReader)
        {
            try
            {
                int chatId = (int)dataReader["ChatId"];
                string chatName = (string)dataReader["ChatName"];
                int membersCount = await GetChatMembersCount(chatId);

                Chat chat = new Chat(
                    chatId, 
                    chatName,
                    membersCount);

                return chat;
            }
            catch
            {
                // При ошибке чтения данных возвращаем null
                return null!;
            }
        }

        public async Task CreateChatAsync(string chatName, int creatorId)
        {
            await ExecuteNonQueryAsync(
                @$"DECLARE @NewChatIdTable TABLE(Id INT);
                   DECLARE @NewChatId INT;

                   INSERT INTO Chats(ChatName, MembersCount) 
                   OUTPUT INSERTED.ChatId INTO @NewChatIdTable(Id)
                   VALUES ('{chatName}', 1)

                   SELECT @NewChatId = Id FROM @NewChatIdTable

                   INSERT INTO UserChats(UserId, ChatId, UserRole) VALUES({creatorId}, @NewChatId, 'Admin');");
        }

        public async Task AddUserToChatAsync(int userId, int chatId)
        {
            await ExecuteNonQueryAsync(
                $@" INSERT INTO UserChats(UserId, ChatId) VALUES({userId}, {chatId});");
        }

        public async Task DeleteUserFromChatAsync(int userId, int chatId)
        {
            // Получение количества участников чата
            int chatMembersCount = await GetChatMembersCount(chatId);

            // Удаление пользователя из чата
            await ExecuteNonQueryAsync($@"DELETE FROM UserChats WHERE UserChats.UserId = {userId} AND UserChats.ChatId = {chatId};");

            // Если пользователь,который был удаён из чата, оказался последним, то чат удаляется из базы данных
            chatMembersCount--;
            if (chatMembersCount <= 0)
                await DeleteChatFromDataBase(chatId);
        }

        public async Task<bool> IsChatContainsMember(int userId, int chatId)
            => await IsSqlQueryEmpty($@"SELECT * FROM UserChats WHERE UserChats.UserId = {userId} AND UserChats.ChatId = {chatId};");

        #endregion

        /// <summary>
        /// Получение набора чатов из SQL-запроса.
        /// </summary>
        /// <param name="sqlQuery">SQL-запрос.</param>
        /// <returns>Набор чатов, полученный из указанного SQL-запроса (<paramref name="sqlQuery"/>).</returns>
        private async Task<IEnumerable<Chat>?> GetChatsListFromSqlQuery(string sqlQuery)
        {
            using SqlConnection sqlConnection = await sqlServer.GetSqlConnectionAsync();

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
                Chat? chat = chats.Find(c => c.ChatId == chatId);

                // Если чат с таким id попался впервые, создаём новый объект класса Chat
                if (chat == null)
                {
                    chat = await GetChatFromReader(dataReader);
                    if (chat != null)
                        chats.Add(chat);
                }

                Message chatMessage = _messagesRepository.GetMessageFromReader(dataReader);
                chat?.IncludeMessage(dataReader, chatMessage);
            }

            return chats;
        }

        /// <summary>
        /// Получение количества участников в чате.
        /// </summary>
        /// <param name="chatId">Id чата, в котором нужно посчитать участников.</param>
        /// <returns>Количество участников в чате.</returns>
        private async Task<int> GetChatMembersCount(int chatId)
        {
            return await ExecuteScalarAsync<int>($@"SELECT COUNT(*) FROM UserChats WHERE UserChats.ChatId = {chatId};");
        }

        /// <summary>
        /// Удаление чата из таблицы Chats в базе данных.
        /// </summary>
        /// <param name="chatId">Id чата, который нужно удалить.</param>
        private async Task DeleteChatFromDataBase(int chatId)
        {
            await ExecuteNonQueryAsync(
                    $"DELETE FROM Messages WHERE Messages.ChatId = {chatId}; " + // Сначала удаляются все сообщения из чата
                    $"DELETE FROM Chats WHERE Chats.ChatId = {chatId};");        // Затем удаляется сам чат
        }
    }
}
