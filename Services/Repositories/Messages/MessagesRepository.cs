using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.MessageDto;
using SoftworkMessanger.Utilites;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Messages
{
    public class MessagesRepository : RepositoryBase, IMessagesRepository
    {
        public MessagesRepository(SqlServerConnector sqlServerConnector) : base(sqlServerConnector) { }


        public Message GetMessageFromReader(IDataReader dataReader)
        {
            try
            {
                int messageId = (int)dataReader["MessageId"];
                int authorId = (int)dataReader["AuthorId"];
                string authorName = (string)dataReader["UserName"];
                string messageText = (string)dataReader["MessageText"];
                int chatId = (int)dataReader["ChatId"];

                Message message = new Message(messageId, authorId, authorName, messageText, chatId);

                return message;
            }
            catch
            {
                // При ошибке чтения вместо сообщения вернётся null
                return null!;
            }
        }

        public async Task AddMessageAsync(NewMessageData newMessageData, int authorId)
        {
            await ExecuteNonQueryAsync(
                @$"INSERT INTO Messages(MessageText, AuthorId, ChatId) 
                   VALUES ('{newMessageData.MessageText}', {authorId}, {newMessageData.ChatId});");
        }
    }
}
