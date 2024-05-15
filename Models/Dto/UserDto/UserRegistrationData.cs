namespace ZeraMessanger.Models.Dto.UserDto
{
    /// <summary>
    /// Данные, которые вводит пользователь в форме регистрации.
    /// </summary>
    /// <param name="Name">Имя пользователя.</param>
    /// <param name="Password">Пароль пользователя.</param>
    /// <param name="EMail">Электронная почта пользователя.</param>
    public record UserRegistrationData(
        string Name,
        string Password,
        string Email);
}
