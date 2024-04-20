using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
using SoftworkMessanger.Utilites;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Messages
{
    public class MessagesRepository : IMessagesRepository
    {
        public MessagesRepository(SqlServerConnector sqlServerConnector)
        {
            _sqlServerConnector = sqlServerConnector;
        }

        private readonly SqlServerConnector _sqlServerConnector;

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
                return null!;
            }
        }

        public void AddMessage(string messageText, int authorId, int chatId)
        {
            ExecuteNonQueryAsync($"INSERT INTO Messages(MessageText, AuthorId, ChatId) VALUES ('{messageText}', {authorId}, {chatId});");
        }

        private async void ExecuteNonQueryAsync(string sqlQuery)
        {
            using SqlConnection sqlConnection = await _sqlServerConnector.GetSqlConnectionAsync();

            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            await sqlCommand.ExecuteNonQueryAsync();
        }
    }
}
