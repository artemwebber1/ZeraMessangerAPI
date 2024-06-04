using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Extensions;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.ChatDto;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories;

namespace ZeraMessanger.Controllers
{
    /// <summary>
    /// Контроллер чатов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ZeraControllerBase
    {
        public ChatsController(
            IUsersRepository usersRepository,
            IChatsRepository chatsRepository,
            IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _usersRepository = usersRepository;
            _chatsRepository = chatsRepository;
        }

        private readonly IUsersRepository _usersRepository;
        private readonly IChatsRepository _chatsRepository;

        #region Actions

        [HttpGet("{chatId:int}")]
        [AllowAnonymous]
        public async Task<Chat?> GetChatByIdAsync(int chatId)
        {
            return await _chatsRepository.GetByIdAsync(chatId);
        }
        
        [HttpGet("FoundedChatsByName")]
        public async Task<IEnumerable<ChatFirstView>> FindChatsByNameAsync(string chatName)
        {
            IEnumerable<Chat> chats = await _chatsRepository.FindChatsWithNameAsync(chatName);
            return chats.ToChatFirstViewCollection();
        }

        [HttpGet("UserChats")]
        public async Task<IEnumerable<ChatFirstView>> GetChatsForUserWithIdAsync()
        {
            return await _chatsRepository.GetUserChatsAsync(IdentityId);
        }


        [HttpPost("CreateChat")]
        public async Task<IResult> CreateChatAsync(string chatName)
        {
            int newChatId = await _chatsRepository.CreateChatAsync(chatName, IdentityId);
            return Results.Ok(newChatId);
        }

        #endregion
    }
}
