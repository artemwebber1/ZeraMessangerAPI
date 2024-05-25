using System.Text.Json.Serialization;

namespace ZeraMessanger.Models
{
    /// <summary>
    /// Модель сообщения, которое пользователь отправляет в какой-либо чат.
    /// </summary>
    public record Message
    {
        /// <summary>
        /// Id сообщения.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string MessageText { get; set; } = string.Empty;

        /// <summary>
        /// Имя автора сообщения.
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Id автора сообщения.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Id чата, в котором находится сообщение.
        /// </summary>
        public int ChatId { get; set; }
    }
}
