using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.ChatDto;

namespace ZeraMessanger.Extensions
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
                chat.Members.Count);

        public static IEnumerable<ChatFirstView> ToChatFirstViewCollection(this IEnumerable<Chat> chats)
        {
            ICollection<ChatFirstView> chatFirstViews = [];

            foreach (var chat in chats) 
                chatFirstViews.Add(chat.ToChatFirstView());

            return chatFirstViews;
        }
    }
}
