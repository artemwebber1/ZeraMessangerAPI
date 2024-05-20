using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ZeraMessanger.Models
{
    /// <summary>
    /// Модель чата приложения.
    /// </summary>
    public record Chat
    {
        /// <summary>
        /// Id чата.
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Название чата. Название может быть изменено в будущем.
        /// </summary>
        public string ChatName { get; set; } = string.Empty;

        /// <summary>
        /// Все сообщения в чате.
        /// </summary>
        public List<Message> Messages { get; set; } = [];

        /// <summary>
        /// Id админа чата.
        /// </summary>
        [JsonIgnore]
        public int AdminId { get; set; }

        /// <summary>
        /// Список участников чата.
        /// </summary>
        [JsonIgnore]
        public List<User> Members { get; set; } = [];
    }
}
