using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;

namespace ZeraMessanger.Services.Repositories
{
    /// <summary>
    /// Репозиторий для работы с таблицей сообщений.
    /// </summary>
    public interface IMessagesRepository
    {
        /// <summary>
        /// Добавление сообщения в чат.
        /// </summary>
        /// <param name="messageData">Данные сообщения.</param>
        /// <param name="authorId">Id автора сообщения.</param>
        /// <returns>Объект класса <see cref="Message"/> с данными добавленного сообщения.</returns>
        Task<Message> AddMessageAsync(NewMessageData messageData, int? authorId);
    }
}
