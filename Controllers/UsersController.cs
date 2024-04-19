using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Services.Repositories.Users;

namespace SoftworkMessanger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        private readonly IUsersRepository _usersRepository;

        [HttpGet("{userId:int}")]
        public User GetUser(int userId)
        {
            return _usersRepository.GetById(userId);
        }
    }
}
