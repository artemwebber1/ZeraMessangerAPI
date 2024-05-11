using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.ChatDto;
using System.Data;

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
        Task<Chat?> GetByIdAsync(int chatId);

        /// <summary>
        /// Получение чатов для конкретного пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя, для которого нужно вернуть чаты.</param>
        /// <returns>Набор чатов для конкретного пользователя.</returns>
        Task<IEnumerable<ChatFirstView>> GetUserChatsAsync(int userId);

        /// <summary>
        /// Получение конкретного чата из читателя данных SQL-запроса.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Объект класса <see cref="Chat"/>, прочитанный из читателя данных SQL-запроса.</returns>
        Task<Chat?> GetChatFromReader(
            IDataReader dataReader,
            string chatIdColumn,
            string chatNameColumn);

        /// <summary>
        /// Добавление нового чата в базу данных.
        /// </summary>
        /// <param name="chatName">Имя нового чата.</param>
        /// <param name="creatorId">Id создателя чата.</param>
        Task CreateChatAsync(string chatName, int creatorId);

        /// <summary>
        /// Добавление пользователя в чат.
        /// </summary>
        /// <param name="userId">Id пользователя, которого нужно добавить в чат.</param>
        /// <param name="chatId">Id чата, куда надо добавить пльзователя.</param>
        Task AddUserToChatAsync(int userId, int chatId);

        /// <summary>
        /// Удаляет пользователя из чата.
        /// </summary>
        /// <param name="userId">Id пользователя, который будет удалён из чата.</param>
        /// <param name="chatId">Id чата, из которого пользователь будет удалён.</param>
        Task DeleteUserFromChatAsync(int userId, int chatId);

        /// <summary>
        /// Проверяет, содержится ли пользователь в конкретном чате.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="chatId">Id чата.</param>
        /// <returns>True если пользователь является участником чата, иначе false.</returns>
        Task<bool> IsChatContainsMember(int userId, int chatId);
    }
}
