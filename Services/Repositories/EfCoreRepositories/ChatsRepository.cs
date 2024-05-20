using Microsoft.EntityFrameworkCore;
using ZeraMessanger.DbContexts;
using ZeraMessanger.Extensions;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.ChatDto;

namespace ZeraMessanger.Services.Repositories.EfCoreRepositories
{
    public class ChatsRepository : IChatsRepository
    {
        public async Task<Chat?> GetByIdAsync(int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat? chat = await dbContext.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(chat => chat.ChatId == chatId) 
                ?? throw new NullReferenceException("Chat doesn't exist");                

            return chat;
        }

        public async Task<int> CreateChatAsync(string chatName, int creatorId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat chat = new Chat
            {
                ChatName = chatName,
                AdminId = creatorId,
            };

            User? chatCreator = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == creatorId);
            if (chatCreator == null)
                throw new NullReferenceException("Uer doesn't exist");

            chat.Members.Add(chatCreator);

            await dbContext.Chats.AddAsync(chat);
            await dbContext.SaveChangesAsync();

            return chat.ChatId;
        }

        public async Task AddUserToChatAsync(int userId, int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat? chat = await dbContext.Chats.FirstOrDefaultAsync(c => c.ChatId == chatId);
            User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (chat == null || user == null)
                throw new NullReferenceException("Chat or user doesn't exist");

            chat.Members.Add(user);

            dbContext.Chats.Update(chat);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserFromChatAsync(int userId, int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat? chat = await dbContext.Chats.Include(c => c.Members).FirstOrDefaultAsync(c => c.ChatId == chatId);
            User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (chat == null || user == null)
                throw new NullReferenceException("Chat or user doesn't exist");

            chat.Members.Remove(user);
            if (chat.Members.Count <= 0)
                await dbContext.Chats.Where(c => c.ChatId == chat.ChatId).ExecuteDeleteAsync();

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatFirstView>> GetUserChatsAsync(int userId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            List<Chat> chats = await dbContext.Chats
                .Include(c => c.Members)
                .Where(c => c.Members.Any(u => u.UserId == userId))
                .AsNoTracking()
                .ToListAsync();

            List<ChatFirstView> chatFirstViews = new List<ChatFirstView>();
            for (int i = 0; i < chats.Count; i++)
            {
                ChatFirstView chatFirstView = chats[i].ToChatFirstView();
                chatFirstViews.Add(chatFirstView);
            }

            return chatFirstViews;
        }

        public async Task<bool> IsChatContainsMember(int userId, int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat? chat = await dbContext.Chats.Include(c => c.Members).FirstOrDefaultAsync(c => c.ChatId == chatId);
            User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (chat == null || user == null)
                throw new NullReferenceException("Chat or user doesn't exist");

            return chat.Members.Contains(user);
        }
    }
}
