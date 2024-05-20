using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.UserDto;
using ZeraMessanger.Services.Authentification;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories;

namespace ZeraMessanger.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ZeraControllerBase
    {
        public AccountController(
            IAuthentificationService authentificationService,
            IUsersRepository usersRepository,
            IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _authentificationService = authentificationService;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Сервис для аутентификации пользователей.
        /// </summary>
        private readonly IAuthentificationService _authentificationService;

        /// <summary>
        /// Репозиторий пользователей.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        #region Actions

        [HttpGet]
        public async Task<User?> GetIdentityAsync()
        {
            User? identity = await _usersRepository.GetByIdAsync(IdentityId);
            return identity;
        }

        [HttpPut("UpdateIdentityData")]
        public async Task UpdateIdentityData(UserUpdateData updateData)
        {
            await _usersRepository.UpdateUserAsync(updateData, userId: IdentityId);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IResult> RegisterNewUser(UserRegistrationData userRegistrationData)
        {
            return await _authentificationService.RegisterNewUserAsync(userRegistrationData);
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IResult> LoginUserAsync(UserLoginData userLoginData)
        {
            return await _authentificationService.LoginUserAsync(userLoginData);
        }

        #endregion
    }
}
