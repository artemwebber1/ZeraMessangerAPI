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
                Message message = new Message
                {
                    MessageId = (int)dataReader["MessageId"],
                    MessageText = (string)dataReader["MessageText"],
                    AuthorId = (int)dataReader["AuthorId"],
                    ChatId = (int)dataReader["ChatId"]
                };

                return message;
            }
            catch
            {
                // При ошибке чтения вместо сообщения вернётся null
                return null!;
            }
        }

        public async Task AddMessageAsync(NewMessageData newMessageData)
        {
            await ExecuteNonQueryAsync(
                @$"INSERT INTO Messages(MessageText, AuthorId, ChatId) 
                   VALUES ('{newMessageData.MessageText}', {newMessageData.AuthorId}, {newMessageData.ChatId});");
        }
    }
}
