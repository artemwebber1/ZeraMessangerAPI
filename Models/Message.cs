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
        public string? AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Id автора сообщения. Если равняется null, то сообщение будет обрабатываться как событийное.
        /// <br/>
        /// Событийное сообщение - сообщение, которое не принадлежит никому из пользователей. 
        /// Оно отправляется само при выполнении какого-либо условия (например, пользователь вышел/зашёл в чат).
        /// </summary>
        public int? AuthorId { get; set; } = null;

        /// <summary>
        /// Id чата, в котором находится сообщение.
        /// </summary>
        public int ChatId { get; set; }
    }
}
