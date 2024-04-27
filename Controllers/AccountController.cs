using Microsoft.AspNetCore.Mvc;

namespace SoftworkMessanger.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost("SignUp")]
        public void RegisterNewUser()
        {

        }

        [HttpPost("SignIn")]
        public void LoginUser()
        {

        }
    }
}
