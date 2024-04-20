namespace SoftworkMessanger.Models
{
    /// <summary>
    /// Модель сообщения, которое пользователь отправляет в какой-либо чат.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Id сообщения.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Id автора сообщения.
        /// </summary>
        public int AuthorId { get; set; }

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
