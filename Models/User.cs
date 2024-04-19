namespace SoftworkMessanger.Models
{
    /// <summary>
    /// Модель пользователя приложения.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Захэшированный пароль пользователя.
        /// </summary>
        public string? UserHashedPassword { get; set; }

        /// <summary>
        /// Эл. почта пользователя.
        /// </summary>
        public string? UserEmail { get; set; }
    }
}
