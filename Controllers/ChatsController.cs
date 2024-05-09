using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.ChatDto;
using SoftworkMessanger.Models.Dto.MessageDto;
using SoftworkMessanger.Services.Repositories.Chats;
using SoftworkMessanger.Services.Repositories.Messages;
using SoftworkMessanger.Services.Authentification.Jwt;

namespace SoftworkMessanger.Controllers
{
    /// <summary>
    /// Контроллер чатов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        public ChatsController(IChatsRepository chatsRepository, IMessagesRepository messagesRepository, IJwtDecoder jwtDecoder)
        {
            _chatsRepository = chatsRepository;
            _messagesRepository = messagesRepository;
            _jwtDecoder = jwtDecoder;
        }

        private readonly IChatsRepository _chatsRepository;
        private readonly IMessagesRepository _messagesRepository;
        
        private readonly IJwtDecoder _jwtDecoder;

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
            int userId = int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));
            return await _chatsRepository.GetUserChatsAsync(userId);
        }

        [HttpPost("CreateChat")]
        public async Task CreateChatAsync(string chatName)
        {
            int chatCreatorId = int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));
            await _chatsRepository.CreateChatAsync(chatName, chatCreatorId);
        }

        [HttpPost("AddUserToChat")]
        public async Task AddUserToChatAsync(int userId, int chatId)
        {
            // Пользователь, которого пытаются добавить, уже состоит в чате?
            bool isUserInChat = await _chatsRepository.IsChatContainsMember(userId, chatId);

            // Приглашающий состоит в чате?
            int inviterId = int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));
            bool isInviterInChat = await _chatsRepository.IsChatContainsMember(inviterId, chatId);

            if (isUserInChat || !isInviterInChat)
            {
                Response.StatusCode = 400;
                return;
            }

            await _chatsRepository.AddUserToChatAsync(userId, chatId);
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

            int excluderId = int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));

            bool isChatContainsUser = await _chatsRepository.IsChatContainsMember(userId, chatId);

            //  Пользователю, не являющемуся админом, запрещено исключать других пользователей.
            bool isExcluderAdmin = await _chatsRepository.IsAdmin(excluderId, chatId);

            if (!isChatContainsUser || (userId != excluderId && !isExcluderAdmin))
                return Results.BadRequest($"Ошибка при исключении пользователя с id = {userId}.");

            await _chatsRepository.DeleteUserFromChatAsync(userId, chatId);
            return Results.Ok($"Пользователь с id = {userId} был исключен из чата {chatId}.");
        }

        [HttpPost("AddMessage")]
        public async Task AddMessage(NewMessageData newMessageData)
        {
            int messageAuthorId = int.Parse(_jwtDecoder.GetClaimValue("UserId", Request));

            //  Пользователь может отправлять сообщения только если он является участником чата
            bool isMessageAuthorInChat = await _chatsRepository.IsChatContainsMember(messageAuthorId, newMessageData.ChatId);
            if (!isMessageAuthorInChat)
            {
                Response.StatusCode = 403;
                return;
            }

            //  В будущем id автора будет доставаться из JWT-токена, когда завезу авторизацию
            await _messagesRepository.AddMessageAsync(newMessageData, messageAuthorId);
        }

        #endregion

    }
}
