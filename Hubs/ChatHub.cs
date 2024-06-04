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


        public async Task ConnectUserToChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

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

        [HubMethodName("DeleteUserFromChat")]
        public async Task DeleteUserFromChatAsync(int userId, int chatId)
        {
            // Удаление пользователя из чата и получение количества оставшихся участников
            int membersRemained = await _chatsRepository.DeleteUserFromChatAsync(userId, chatId);

            if (membersRemained > 0)
                await Clients.Group(chatId.ToString()).SendAsync("OnUserLeftChat");
        }

        public async Task AddMessageToChat(string messageText, int chatId, int? authorId)
        {
            NewMessageData messageData = new NewMessageData(chatId, messageText);

            // Добавление сообщения в базу данных
            Message message = await _messagesRepository.AddMessageAsync(messageData, authorId);
            // Уведомление всех клиентов, состоящих в группе
            await Clients.Group(chatId.ToString()).SendAsync("OnMessageSent", messageText, message.AuthorId, message.AuthorName);
        }
    }
}
