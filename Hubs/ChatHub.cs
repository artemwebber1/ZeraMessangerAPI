using Microsoft.AspNetCore.SignalR;
using ZeraMessanger.Models.Dto.MessageDto;
using ZeraMessanger.Services.Repositories;

namespace SoftworkMessanger.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub(IMessagesRepository messagesRepository)
        {
            _messagesRepository = messagesRepository;
        }

        private readonly IMessagesRepository _messagesRepository;

        public async Task AddUserToChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task AddMessageToChat(string messageText, string authorId, string chatId)
        {
            NewMessageData messageData = new NewMessageData(int.Parse(chatId), messageText);
            await _messagesRepository.AddMessageAsync(messageData, int.Parse(authorId));

            await Clients.Group(chatId).SendAsync("OnMessageSent", messageText, authorId);
        }
    }
}
