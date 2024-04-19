using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;
using SoftworkMessanger.Services.Repositories.Chats;

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

        /// <summary>
        /// Репозиторий для работы с таблицей чатов в базе данных.
        /// </summary>
        private readonly IChatsRepository _chatsRepository;

        #region Actions

        [HttpGet("{chatId:int}")]
        public Chat? GetChat(int chatId)
        {
            return _chatsRepository.GetById(chatId);
        }

        [HttpGet("userChat")]
        public IEnumerable<Chat>? GetChatsForUserWithId(int userId)
        {
            return _chatsRepository.GetUserChats(userId);
        }

        #endregion
    }
}
