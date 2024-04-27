using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.ChatDto;
using SoftworkMessanger.Models.Dto.MessageDto;
using SoftworkMessanger.Services.Repositories.Chats;
using SoftworkMessanger.Services.Repositories.Messages;

namespace SoftworkMessanger.Controllers
{
    /// <summary>
    /// Контроллер чатов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        public ChatsController(IChatsRepository chatsRepository)
        {
            _chatsRepository = chatsRepository;
        }

        private readonly IChatsRepository _chatsRepository;

        #region Actions

        [HttpGet("{chatId:int}")]
        public async Task<IEnumerable<Chat>?> GetChatByIdAsync(int chatId)
        {
            return await _chatsRepository.GetByIdAsync(chatId);
        }

        [HttpGet("UserChats")]
        public async Task<IEnumerable<ChatFirstView>> GetChatsForUserWithIdAsync(int userId)
        {
            return await _chatsRepository.GetUserChatsAsync(userId);
        }

        [HttpPost("CreateChat")]
        public void CreateChat(NewChatData newChatData)
        {
            _chatsRepository.CreateChatAsync(newChatData);
        }

        [HttpPost("AddUserToChat")]
        public async Task AddUserToChatAsync(int userId, int inviterId, int chatId)
        {
            // Пользователь, которого пытаются добавить, уже состоит в чате?
            bool isUserInChat = await _chatsRepository.IsChatContainsMember(userId, chatId);
            // Приглашающий состоит в чате?
            bool isInviterInChat = await _chatsRepository.IsChatContainsMember(inviterId, chatId);

            if (isUserInChat || !isInviterInChat)
            {
                Response.StatusCode = 400;
                return;
            }

            await _chatsRepository.AddUserToChatAsync(userId, chatId);
        }

        [HttpDelete("DeleteUserFromChat")]
        public async Task<IResult> DeleteUserFromChatAsync(int userId, int excluderId, int chatId)
        {
            //  Пользователь удаляется из чата, если выполняется одно из условийй:
            //      1. Пользователь сам решил выйти из чата (userId == excluderId)
            //      
            //      2. Пользователя исключает админ чата.
            //         В этом случае проверяется, является ли пользователь,
            //         исключающий другого пользователя, админом чата,
            //      -------
            //      В обоих случаях проверяется, состоит ли пользователь, которого нужно исключить, в чате.

            bool isChatContainsUser = await _chatsRepository.IsChatContainsMember(userId, chatId);
            bool isExcluderAdmin = await _chatsRepository.IsAdmin(excluderId, chatId);

            if (!isChatContainsUser || (userId != excluderId && !isExcluderAdmin))
                //  Пользователю, не являющемуся админом, запрещено исключать других пользователей.
                return Results.BadRequest($"Ошибка при исключении пользователя с id = {userId}.");

            await _chatsRepository.DeleteUserFromChatAsync(userId, chatId);
            return Results.Ok($"Пользователь с id = {userId} был исключен из чата {chatId}.");
        }


        [HttpPost("AddMessage")]
        public async Task AddMessage(NewMessageData newMessageData, IMessagesRepository messagesRepository)
        {
            //  Пользователь может отправлять сообщения только если он является участником чата
            bool isMessageAuthorInChat = await _chatsRepository.IsChatContainsMember(newMessageData.AuthorId, newMessageData.ChatId);
            if (!isMessageAuthorInChat)
            {
                Response.StatusCode = 403;
                return;
            }

            //  В будущем id автора будет доставаться из JWT-токена, когда завезу авторизацию
            await messagesRepository.AddMessageAsync(newMessageData);
        }

        #endregion
    }
}
