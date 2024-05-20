using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;
using System.Data;

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
        /// <param name="newMessageData">Данные сообщения.</param>
        /// <param name="authorId">Id автора сообщения.</param>
        /// <returns>Id нового сообщения.</returns>
        Task<int> AddMessageAsync(NewMessageData newMessageData, int authorId);
    }
}
