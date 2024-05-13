using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Models.Dto.UserDto;
using ZeraMessanger.Services.Authentification;

namespace ZeraMessanger.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }

        private readonly IAuthentificationService _authentificationService;

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
    }
}
