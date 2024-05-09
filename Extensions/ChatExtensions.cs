using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.ChatDto;
using SoftworkMessanger.Services.Repositories.Messages;
using System.Data;

namespace SoftworkMessanger.Extensions
{
    public static class ChatExtensions
    {
        /// <summary>
        /// Добавление сообщения чата объекту класса <see cref="Chat"/>.
        /// </summary>
        /// <param name="chat">Объект класса <see cref="Chat"/>, которому нужно добавить сообщение.</param>
        /// <param name="dataReader">Читатель данных для получения данных о сообщении.</param>
        /// <param name="messagesRepository">Репозиторий сообщений для работы с соответствующей таблицей в базе данных.</param>
        /// <returns>Тот же объект класса <see cref="Chat"/>, но с добавленным к нему сообщением.</returns>
        public static Chat IncludeMessage(this Chat chat, IDataReader dataReader, Message message)
        {
            if (Convert.IsDBNull(dataReader["MessageId"]))  // Если в чате нет сообщений - ничего не делаем, выходим из метода
                return chat;

            chat.Messages.Add(message);

            return chat;
        }

        /// <summary>
        /// Преобразование модели <see cref="Chat"/> в DTO-модель <see cref="ChatFirstView"/>.
        /// </summary>
        /// <param name="chat">Объект класса <see cref="Chat"/>, который надо преобразовать в <see cref="ChatFirstView"/>.</param>
        /// <returns>Объект класса <see cref="ChatFirstView"/>.</returns>
        public static ChatFirstView ToChatFirstView(this Chat chat)
            => new ChatFirstView(
                chat.ChatId,
                chat.ChatName,
                chat.MembersCount);
    }
}
