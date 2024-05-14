using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeraMessanger.Models.Dto.MessageDto;
using ZeraMessanger.Services.Authentification.Jwt;
using ZeraMessanger.Services.Repositories.Chats;
using ZeraMessanger.Services.Repositories.Messages;

namespace ZeraMessanger.Controllers
{
    [Authorize]
    [Route("Api/[controller]")]
    [ApiController]
    public class MessagesController : ZeraControllerBase
    {
        public MessagesController(IMessagesRepository messagesRepository, IChatsRepository chatsRepository, IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _messagesRepository = messagesRepository;
            _chatsRepository = chatsRepository;
        }

        private readonly IMessagesRepository _messagesRepository;
        private readonly IChatsRepository _chatsRepository;

        #region Actions

        [HttpPost("AddMessage")]
        public async Task<IResult> AddMessage(NewMessageData newMessageData)
        {
            int messageAuthorId = IdentityId;

            //  Пользователь может отправлять сообщения только если он является участником чата
            bool isMessageAuthorInChat = await _chatsRepository.IsChatContainsMember(messageAuthorId, newMessageData.ChatId);
            if (!isMessageAuthorInChat)
                return Results.Forbid();

            await _messagesRepository.AddMessageAsync(newMessageData, messageAuthorId);
            return Results.Ok();
        }

        #endregion
    }
}
