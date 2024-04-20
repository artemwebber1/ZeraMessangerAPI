using SoftworkMessanger.Models;
using System.Data;

namespace SoftworkMessanger.Services.Repositories.Messages
{
    /// <summary>
    /// Репозиторий для работы с таблицей сообщений.
    /// </summary>
    public interface IMessagesRepository
    {
        /// <summary>
        /// Добавление нового сообщения в чат с указанным id <paramref name="chatId"/>.
        /// </summary>
        /// <param name="messageText">Текст сообщения, которое нужно добавить.</param>
        /// <param name="authorId">Id пользователя, который отправил сообщение.</param>
        /// <param name="chatId">Id чата, в котором будет находиться сообщение.</param>
        void AddMessage(string messageText, int authorId, int chatId);

        /// <summary>
        /// Получение объекта класса <see cref="Message"/> из читателя данных.
        /// </summary>
        /// <param name="dataReader">Читатель данных.</param>
        /// <returns>Объект класса <see cref="Message"/>, полученный из читателя данных.</returns>
        Message GetMessageFromReader(IDataReader dataReader);
    }
}
