using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.MessageDto;
using SoftworkMessanger.Utilites;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Messages
{
    public class MessagesRepository : RepositoryBase, IMessagesRepository
    {
        public MessagesRepository(SqlServerConnector sqlServerConnector) : base(sqlServerConnector) { }


        public Message GetMessageFromReader(IDataReader dataReader, string authorIdColumn, string authorNameColumn, string messageTextColumn)
        {
            try
            {
                int authorId = (int)dataReader[authorIdColumn];
                string authorName = (string)dataReader[authorNameColumn];
                string messageText = (string)dataReader[messageTextColumn];

                Message message = new Message(authorId, authorName, messageText);
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
