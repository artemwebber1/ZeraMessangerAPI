using System.Text.Json.Serialization;

namespace ZeraMessanger.Models
{
    /// <summary>
    /// Модель пользователя приложения.
    /// </summary>
    public record User
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Захэшированный пароль пользователя.
        /// </summary>
        public string UserPassword { get; set; } = string.Empty;

        /// <summary>
        /// Эл. почта пользователя.
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Чаты пользователя.
        /// </summary>
        [JsonIgnore]
        public List<Chat> Chats { get; set; } = [];
    }
}
