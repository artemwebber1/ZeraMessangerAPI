namespace ZeraMessanger.Models
{
    /// <summary>
    /// Модель пользователя приложения.
    /// </summary>
    public record User
    {
        public User(int userId, string userName, string hashedPassword, string email)
        {
            UserId = userId;
            UserName = userName;
            UserHashedPassword = hashedPassword;
            UserEmail = email;
        }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; } = null!;

        /// <summary>
        /// Захэшированный пароль пользователя.
        /// </summary>
        public string UserHashedPassword { get; } = null!;

        /// <summary>
        /// Эл. почта пользователя.
        /// </summary>
        public string UserEmail { get; } = null!;
    }
}
