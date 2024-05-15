using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories.Users;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.UserDto;

namespace ZeraMessanger.Services.Authentification
{
    public class AuthentificationService : IAuthentificationService
    {
        public AuthentificationService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtGenerator jwtGenerator)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
        }

        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtGenerator _jwtGenerator;

        #region IAuthentificationService implementation

        public async Task<IResult> RegisterNewUserAsync(UserRegistrationData userRegistrationData)
        {
            // Проверяем, существует ли уже такой пользователь
            bool isUserWithEmailExists = await _usersRepository.IsUserExistsWithEmail(userRegistrationData.Email);
            if (isUserWithEmailExists)
                // Если да, возвращаем ошибку
                return Results.Forbid();

            // Если нет, добавить пользователя в базу данных, предварительно зашифровав его пароль
            string hashedPassword = _passwordHasher.Generate(userRegistrationData.Password);
            await _usersRepository.AddUserAsync(userRegistrationData.Name, hashedPassword, userRegistrationData.Email);
            return Results.Ok();
        }

        public async Task<IResult> LoginUserAsync(UserLoginData userLoginData)
        {
            // Проверяем, существует ли уже такой пользователь
            User? user = await _usersRepository.GetByEmailAsync(userLoginData.Email);

            // Если пользователя с указанной электронной почтой не сущестует или пароли не совпадают, бросаем ошибку
            if (user == null || !_passwordHasher.Verify(userLoginData.Password, user.UserHashedPassword))
                return Results.Forbid();

            // Пароли совпадают - генерируем JWT-токен и возвращаем его
            // Инициализация клэимов
            Claim[] claims =
            [
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserEmail", user.UserEmail)
            ];

            // Создание токена
            JwtSecurityToken token = _jwtGenerator.GenerateToken(claims);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Results.Json(tokenString);
        }

        #endregion

    }
}
