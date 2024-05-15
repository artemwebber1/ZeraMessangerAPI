using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Models.Dto.UserDto;
using ZeraMessanger.Services.Authentification;
using ZeraMessanger.Services.Authentification.Jwt;

namespace ZeraMessanger.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountController : ZeraControllerBase
    {
        public AccountController(
            IAuthentificationService authentificationService, 
            IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _authentificationService = authentificationService;
        }

        /// <summary>
        /// Сервис для аутентификации пользователей.
        /// </summary>
        private readonly IAuthentificationService _authentificationService;

        #region Actions

        [HttpPost("SignUp")]
        public async Task<IResult> RegisterNewUser(UserRegistrationData userRegistrationData)
        {
            return await _authentificationService.RegisterNewUserAsync(userRegistrationData);
        }

        [HttpPost("SignIn")]
        public async Task<IResult> LoginUserAsync(UserLoginData userLoginData)
        {
            return await _authentificationService.LoginUserAsync(userLoginData);
        }

        #endregion
    }
}
