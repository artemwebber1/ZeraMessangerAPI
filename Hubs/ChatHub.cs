using Microsoft.AspNetCore.SignalR;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;
using ZeraMessanger.Services.Repositories;


namespace ZeraMessanger.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub(IChatsRepository chatsRepository, IMessagesRepository messagesRepository)
        {
            _chatsRepository = chatsRepository;
            _messagesRepository = messagesRepository;
        }

        private readonly IMessagesRepository _messagesRepository;
        private readonly IChatsRepository _chatsRepository;

        /// <summary>
        /// Подключение пользователя к чату (не добавление!).
        /// </summary>
        /// <param name="chatId">Id чата, к которому подключается пользователь.</param>
        [HubMethodName("ConnectUserToChat")]
        public async Task ConnectUserToChatAsync(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        /// <summary>
        /// Добавление пользователя в чат.
        /// </summary>
        /// <param name="userId">Id пользователя, который будет добавлен в чат.</param>
        /// <param name="chatId">Id чата, в который будет добавлен пользователь.</param>
        [HubMethodName("AddUserToChat")]
        public async Task AddUserToChatAsync(int userId, int chatId)
        {
            // Проверяем, состоит ли уже пользователь в чате, в который его надо добавить
            bool isUserInChat = await _chatsRepository.IsChatContainsMember(userId, chatId);
            if (isUserInChat)
                return;

            // Добавляем пользователя в чат
            User user = await _chatsRepository.AddUserToChatAsync(userId, chatId);
            // Уведомление всех пользователей чата
            await Clients.Group(chatId.ToString()).SendAsync("OnUserJoinedChat", user.UserName);
        }

        /// <summary>
        /// Удаление пользователя из чата.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [HubMethodName("DeleteUserFromChat")]
        public async Task DeleteUserFromChatAsync(int userId, int chatId)
        {
            int membersRemained = await _chatsRepository.DeleteUserFromChatAsync(userId, chatId);

            // Сообщение о выходе пользователя из чата будет добавлено если в чате остались пользователи.
            // Нет смысла отправлять сообщение, если некому будет на него смотреть
            if (membersRemained > 0)
                await Clients.Group(chatId.ToString()).SendAsync("OnUserLeftChat");
        }

        /// <summary>
        /// Добавление сообщения в чат.
        /// </summary>
        /// <param name="messageText">Текст сообщения.</param>
        /// <param name="chatId">Id чата, в который будет добавлено сообщение.</param>
        /// <param name="authorId">Id автора сообщения.</param>
        [HubMethodName("AddMessageToChat")]
        public async Task AddMessageToChatAsync(string messageText, int chatId, int? authorId)
        {
            NewMessageData messageData = new NewMessageData(chatId, messageText);

            // Добавление сообщения в базу данных
            Message message = await _messagesRepository.AddMessageAsync(messageData, authorId);
            // Уведомление всех клиентов, состоящих в группе
            await Clients.Group(chatId.ToString()).SendAsync("OnMessageSent", messageText, message.AuthorId, message.AuthorName);
        }
    }
}
