using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto;
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
        public ChatsController(IChatsRepository chatsRepository, IMessagesRepository messagesRepository)
        {
            _chatsRepository = chatsRepository;
            _messagesRepository = messagesRepository;
        }

        /// <summary>
        /// Репозиторий для работы с таблицей чатов в базе данных.
        /// </summary>
        private readonly IChatsRepository _chatsRepository;
        private readonly IMessagesRepository _messagesRepository;

        #region Actions

        [HttpGet("{chatId:int}")]
        public IEnumerable<Chat>? GetChat(int chatId)
        {
            return _chatsRepository.GetById(chatId);
        }

        [HttpGet("UserChat")]
        public IEnumerable<ChatFirstView>? GetChatsForUserWithId(int userId)
        {
            return _chatsRepository.GetUserChats(userId);
        }

        [HttpPost("AddMessage/{messageText}")]
        public void AddMessage(string messageText, int authorId, int chatId)
        {
            // В будущем id автора будет доставаться из JWT-токена, когда завезу авторизацию
            _messagesRepository.AddMessage(messageText, authorId, chatId);
        }

        #endregion
    }
}
