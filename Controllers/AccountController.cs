using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models.Dto.UserDto;
using SoftworkMessanger.Services.Authentification;

namespace SoftworkMessanger.Controllers
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
        public async Task RegisterNewUser(UserRegistrationData userRegistrationData)
        {
            await _authentificationService.RegisterNewUserAsync(userRegistrationData);
        }

        [HttpPost("SignIn")]
        public async Task<IResult> LoginUserAsync(UserLoginData userLoginData)
        {
            return await _authentificationService.LoginUserAsync(userLoginData);
        }
    }
}
