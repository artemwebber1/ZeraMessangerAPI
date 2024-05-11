using SoftworkMessanger.Models.Dto.MessageDto;

namespace SoftworkMessanger.Models
{
    /// <summary>
    /// Модель чата приложения.
    /// </summary>
    public record Chat
    {
        public Chat(int id, string chatName, int membersCount)
        {
            ChatId = id;
            ChatName = chatName;
            MembersCount = membersCount;
            Messages = new List<Message>();
        }

        /// <summary>
        /// Id чата.
        /// </summary>
        public int ChatId { get; }

        /// <summary>
        /// Название чата. Название может быть изменено в будущем.
        /// </summary>
        public string ChatName { get; }

        /// <summary>
        /// Количество участников чата.
        /// </summary>
        public int MembersCount { get; }

        /// <summary>
        /// Все сообщения в чате.
        /// </summary>
        public List<Message> Messages { get; }
    }
}
