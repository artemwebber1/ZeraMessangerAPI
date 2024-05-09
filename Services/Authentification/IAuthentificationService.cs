using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models.Dto.UserDto;

namespace SoftworkMessanger.Services.Authentification
{
    /// <summary>
    /// Сервис, отвечающий за аутентификацию пользователей.
    /// </summary>
    public interface IAuthentificationService
    {
        /// <summary>
        /// Регистрация нового пользователя в приложении.
        /// </summary>
        /// <param name="userRegistrationData">Данные пользователя.</param>
        Task<IResult> RegisterNewUserAsync(UserRegistrationData userRegistrationData);

        /// <summary>
        /// Вход зарегистрированного пользователя в систему.
        /// </summary>
        /// <param name="userLoginData">Данные пользователя.</param>
        /// <returns>JWT-токен в формате headers.payload.signature.</returns>
        Task<IResult> LoginUserAsync(UserLoginData userLoginData);
    }
}
