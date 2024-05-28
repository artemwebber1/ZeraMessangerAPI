using Microsoft.AspNetCore.SignalR;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;
using ZeraMessanger.Services.Repositories;

namespace ZeraMessanger.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub(IMessagesRepository messagesRepository)
        {
            _messagesRepository = messagesRepository;
        }

        private readonly IMessagesRepository _messagesRepository;


        public async Task ConnectUserToChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
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
