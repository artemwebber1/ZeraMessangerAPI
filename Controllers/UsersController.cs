﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.UserDto;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories;

namespace ZeraMessanger.Controllers
{
    /// <summary>
    /// Контроллер пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ZeraControllerBase
    {
        public UsersController(
            IUsersRepository usersRepository, 
            IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Репозиторий пользователей для работы с соответствующей таблицей в базе данных.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        #region Actions

        [HttpGet("{userId:int}")]
        public async Task<User?> GetUserAsync(int userId)
        {
            return await _usersRepository.GetByIdAsync(userId);
        }

        #endregion        
    }
}
