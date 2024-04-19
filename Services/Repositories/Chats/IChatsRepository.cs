using SoftworkMessanger.Models;

namespace SoftworkMessanger.Services.Repositories.Chats
{
    /// <summary>
    /// Репозиторий для работы с таблицей чатов в базе данных.
    /// </summary>
    public interface IChatsRepository
    {
        /// <summary>
        /// Получение чата по id.
        /// </summary>
        /// <param name="chatId">Id чата, который нужно вернуть.</param>
        /// <returns>Чат с указанным <paramref name="chatId"/>.</returns>
        Chat? GetById(int chatId);

        /// <summary>
        /// Получение чатов для конкретного пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя, для которого нужно вернуть чаты.</param>
        /// <returns>Набор чатов для конкретного пользователя.</returns>
        IEnumerable<Chat>? GetUserChats(int userId);
    }
}
