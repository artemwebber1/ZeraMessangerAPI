using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Controllers;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.ChatDto;
using ZeraMessanger.Models.Dto.MessageDto;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories.Chats;
using ZeraMessanger.Services.Repositories.Messages;
using ZeraMessanger.Services.Repositories.Users;

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
            {
                return Results.Forbid();
            }

            await _chatsRepository.AddUserToChatAsync(userId, chatId);
            return Results.Ok();
        }

        [HttpDelete("DeleteUserFromChat")]
        public async Task<IResult> DeleteUserFromChatAsync(int userId, int chatId)
        {
            //  Пользователь удаляется из чата, если выполняется одно из условийй:
            //      1. Пользователь сам решил выйти из чата (userId == excluderId)
            //      
            //      2. Пользователя исключает админ чата.
            //         В этом случае проверяется, является ли пользователь,
            //         исключающий другого пользователя, админом чата,
            //      -------
            //      В обоих случаях проверяется, состоит ли пользователь, которого нужно исключить, в чате.

            int excluderId = IdentityId;

            bool isChatContainsUser = await _chatsRepository.IsChatContainsMember(userId, chatId);

            //  Пользователю, не являющемуся админом, запрещено исключать других пользователей
            bool isExcluderAdmin = await _usersRepository.IsAdmin(excluderId, chatId);

            if (!isChatContainsUser || (userId != excluderId && !isExcluderAdmin))
                return Results.BadRequest($"Ошибка при исключении пользователя с id = {userId}.");

            await _chatsRepository.DeleteUserFromChatAsync(userId, chatId);
            return Results.Ok($"Пользователь с id = {userId} был исключен из чата {chatId}.");
        }

        #endregion
    }
}
