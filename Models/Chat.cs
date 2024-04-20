namespace SoftworkMessanger.Models
{
    /// <summary>
    /// Модель чата приложения.
    /// </summary>
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
        }

        /// <summary>
        /// Id чата.
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Название чата. Название может быть изменено в будущем.
        /// </summary>
        public string ChatName { get; set; } = null!;

        /// <summary>
        /// Все сообщения в чате.
        /// </summary>
        public List<Message> Messages { get; }
    }
}
