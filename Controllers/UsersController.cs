using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Services.Authentification.Jwt;
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
        public UsersController(IUsersRepository usersRepository, IJwtDecoder jwtDecoder)
        {
            _usersRepository = usersRepository;
            _jwtDecoder = jwtDecoder;
        }

        /// <summary>
        /// Репозиторий пользователей для работы с соответствующей таблицей в базе данных.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        private readonly IJwtDecoder _jwtDecoder;

        #region Actions

        [HttpGet("{userId:int}")]
        public async Task<User?> GetUserAsync(int userId)
        {
            return await _usersRepository.GetByIdAsync(userId);
        }

        [Authorize]
        [HttpGet("Identity")]
        public async Task<User?> GetIdentityAsync()
        {
            int userId = int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));
            return await _usersRepository.GetByIdAsync(userId);
        }

        #endregion
        
    }
}
