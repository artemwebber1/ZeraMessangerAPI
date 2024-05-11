using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.ChatDto;
using SoftworkMessanger.Services.Repositories.Messages;
using System.Data;

namespace SoftworkMessanger.Extensions
{
    public static class ChatExtensions
    {
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
