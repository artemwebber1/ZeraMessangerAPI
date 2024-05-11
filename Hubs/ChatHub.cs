using Microsoft.AspNetCore.SignalR;

namespace SoftworkMessanger.Hubs
{
    public class ChatHub : Hub
    {
        public async Task AddUserToChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task AddMessageToChat(string messageText, string chatId)
        {
            await Clients.All.SendAsync("OnMessageSent", messageText);
        }
    }
}
