using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Services.Repositories.Users;

namespace SoftworkMessanger.Controllers
{
    /// <summary>
    /// Контроллер пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Репозиторий пользователей для работы с соответствующей таблицей в базе данных.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        #region Actions

        [HttpGet("{userId:int}")]
        public User? GetUser(int userId)
        {
            return _usersRepository.GetById(userId);
        }

        [HttpGet("{userName}")]
        public User? GetUser(string userName)
        {
            return _usersRepository.GetByUserName(userName);
        }

        #endregion
        
    }
}
