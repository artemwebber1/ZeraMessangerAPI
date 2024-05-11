namespace SoftworkMessanger.Models.Dto.UserDto
{
    /// <summary>
    /// Данные, которые пользователь вводит в форме логина.
    /// </summary>
    /// <param name="Email">Электронная почта пользователя.</param>
    /// <param name="Password">Пароль пользователя.</param>
    public record UserLoginData(
        string Email,
        string Password);
}
