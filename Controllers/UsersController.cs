using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.UserDto;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories.Users;

namespace ZeraMessanger.Controllers
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

        private int IdentityId => int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));

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
            return await _usersRepository.GetByIdAsync(IdentityId);
        }

        [Authorize]
        [HttpPut("UpdateIdentityData")]
        public async Task UpdateIdentityData(UserUpdateData updateData)
        {
            await _usersRepository.UpdateUserAsync(updateData, userId: IdentityId);
        }

        #endregion
        
    }
}
