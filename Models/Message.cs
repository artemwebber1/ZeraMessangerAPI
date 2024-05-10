namespace SoftworkMessanger.Models
{
    /// <summary>
    /// Модель сообщения, которое пользователь отправляет в какой-либо чат.
    /// </summary>
    public record Message
    {
        public Message(int messageId, int authorId, string authorName, string messageText, int chatId)
        {
            MessageId = messageId;
            AuthorId = authorId; 
            AuthorName = authorName;
            MessageText = messageText;
            ChatId = chatId;
        }

        /// <summary>
        /// Id сообщения.
        /// </summary>
        public int MessageId { get; }

        /// <summary>
        /// Id автора сообщения.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Имя автора сообщения.
        /// </summary>
        public string AuthorName { get; set; } = null!;

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string MessageText { get; set; } = null!;

        /// <summary>
        /// Id чата, в котором находится сообщение.
        /// </summary>
        public int ChatId { get; set; }
    }
}
