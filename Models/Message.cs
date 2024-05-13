namespace ZeraMessanger.Models
{
    /// <summary>
    /// Модель сообщения, которое пользователь отправляет в какой-либо чат.
    /// </summary>
    public record Message
    {
        public Message(int authorId, string authorName, string messageText)
        {
            AuthorId = authorId; 
            AuthorName = authorName;
            MessageText = messageText;
        }

        /// <summary>
        /// Id автора сообщения.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Имя автора сообщения.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string MessageText { get; set; }
    }
}
