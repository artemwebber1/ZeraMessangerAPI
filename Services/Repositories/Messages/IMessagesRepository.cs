using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;
using System.Data;

namespace ZeraMessanger.Services.Repositories.Messages
{
    /// <summary>
    /// Репозиторий для работы с таблицей сообщений.
    /// </summary>
    public interface IMessagesRepository
    {
        /// <summary>
        /// Получение объекта класса <see cref="Message"/> из читателя данных.
        /// </summary>
        /// <param name="dataReader">Читатель данных.</param>
        /// <returns>Объект класса <see cref="Message"/>, полученный из читателя данных.</returns>
        Message GetMessageFromReader(IDataReader dataReader, string authorIdColumn, string authorNameColumn, string messageTextColumn);

        /// <summary>
        /// Добавление сообщения в чат.
        /// </summary>
        /// <param name="newMessageData">Данные сообщения.</param>
        /// <param name="authorId">Id автора сообщения.</param>
        Task AddMessageAsync(NewMessageData newMessageData, int authorId);
    }
}
