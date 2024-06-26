﻿using System.ComponentModel.DataAnnotations.Schema;
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
        /// Название чата.
        /// </summary>
        public string ChatName { get; set; } = string.Empty;

        /// <summary>
        /// Все сообщения в чате.
        /// </summary>
        public List<Message> Messages { get; set; } = [];

        /// <summary>
        /// Список участников чата.
        /// </summary>
        [JsonIgnore]
        public List<User> Members { get; set; } = [];
    }
}
