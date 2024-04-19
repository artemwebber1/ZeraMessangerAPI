namespace SoftworkMessanger.Models
{
    /// <summary>
    /// Модель чата приложения.
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Id чата.
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Название чата. Название может быть изменено в будущем.
        /// </summary>
        public string? ChatName { get; set; }

        /// <summary>
        /// Все сообщения в чате.
        /// </summary>
        public IEnumerable<Message>? Messages { get; }

        /// <summary>
        /// Участники чата.
        /// </summary>
        public IEnumerable<User>? ChatMembers { get; }
    }
}
