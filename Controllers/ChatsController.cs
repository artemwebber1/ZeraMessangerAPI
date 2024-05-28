using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZeraMessanger.Hubs;
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
            IHubContext<ChatHub> chatHub,
            IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _usersRepository = usersRepository;
            _chatsRepository = chatsRepository;

            _chatHub = chatHub;
        }

        private readonly IUsersRepository _usersRepository;
        private readonly IChatsRepository _chatsRepository;

        private readonly IHubContext<ChatHub> _chatHub;

        #region Actions

        [HttpGet("{chatId:int}")]
        [AllowAnonymous]
        public async Task<Chat?> GetChatByIdAsync(int chatId)
        {
            return await _chatsRepository.GetByIdAsync(chatId);
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


        [HttpPost("AddUserToChat")]
        public async Task<IResult> AddUserToChatAsync(int userId, int chatId)
        {
            // Пользователь, которого пытаются добавить, уже состоит в чате?
            bool isUserInChat = await _chatsRepository.IsChatContainsMember(userId, chatId);

            // Приглашающий состоит в чате?
            int inviterId = IdentityId;
            bool isInviterInChat = await _chatsRepository.IsChatContainsMember(inviterId, chatId);

            if (isUserInChat || !isInviterInChat)
                return Results.Forbid();

            User user = await _chatsRepository.AddUserToChatAsync(userId, chatId);
            await _chatHub.Clients.Group(chatId.ToString()).SendAsync("OnUserJoinedChat", user.UserName);

            return Results.Ok();
        }

        [HttpDelete("DeleteUserFromChat")]
        public async Task<IResult> DeleteUserFromChatAsync(int userId, int chatId)
        {
            await _chatsRepository.DeleteUserFromChatAsync(userId, chatId);
            await _chatHub.Clients.Group(chatId.ToString()).SendAsync("OnUserLeftChat");

            return Results.Ok(userId);
        }

        #endregion
    }
}
