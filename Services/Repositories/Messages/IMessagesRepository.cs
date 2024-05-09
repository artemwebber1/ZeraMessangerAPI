using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.MessageDto;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Messages
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
        Message GetMessageFromReader(IDataReader dataReader);

        /// <summary>
        /// Добавление сообщения в чат.
        /// </summary>
        /// <param name="newMessageData">Данные сообщения.</param>
        /// <param name="authorId">Id автора сообщения.</param>
        Task AddMessageAsync(NewMessageData newMessageData, int authorId);
    }
}
