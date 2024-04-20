using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto;

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
        IEnumerable<Chat>? GetById(int chatId);

        /// <summary>
        /// Получение чатов для конкретного пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя, для которого нужно вернуть чаты.</param>
        /// <returns>Набор чатов для конкретного пользователя.</returns>
        IEnumerable<ChatFirstView>? GetUserChats(int userId);

        /// <summary>
        /// Получение конкретного чата из читателя данных SQL-запроса.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Объект класса <see cref="Chat"/>, прочитанный из читателя данных SQL-запроса.</returns>
        Chat GetChatFromReader(SqlDataReader dataReader);
    }
}
