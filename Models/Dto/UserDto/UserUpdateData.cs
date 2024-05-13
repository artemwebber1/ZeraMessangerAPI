namespace ZeraMessanger.Models.Dto.UserDto
{
    /// <summary>
    /// Данные, которые может обновить пользователь.
    /// </summary>
    /// <param name="UserName">Имя пользователя.</param>
    /// <param name="UserPassword">Пароль пользователя (не захэшированный).</param>
    /// <param name="UserEmail">Электронная почта пользователя.</param>
    public record UserUpdateData(
        string UserName,
        string UserPassword,
        string UserEmail);
}
