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
                .AsNoTracking()
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(chat => chat.ChatId == chatId) 
                ?? throw new NullReferenceException("Chat doesn't exist");

            return chat;
        }

        public async Task<IEnumerable<ChatFirstView>> GetUserChatsAsync(int userId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            // Получаем чаты пользователя
            List<Chat> chats = await dbContext.Chats
                .AsNoTracking()
                .Include(c => c.Members)
                .Where(c => c.Members.Any(u => u.UserId == userId))
                .ToListAsync();

            // Преобразуем чаты в DTO-модели
            List<ChatFirstView> chatFirstViews = new List<ChatFirstView>();
            for (int i = 0; i < chats.Count; i++)
            {
                ChatFirstView chatFirstView = chats[i].ToChatFirstView();
                chatFirstViews.Add(chatFirstView);
            }

            return chatFirstViews;
        }

        public async Task<int> CreateChatAsync(string chatName, int creatorId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat chat = new Chat
            {
                ChatName = chatName,
            };

            // Пользователь, создавший чат, автоматически добавляется в него
            User? chatCreator = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == creatorId);
            if (chatCreator == null)
                throw new NullReferenceException("User doesn't exist");

            chat.Members.Add(chatCreator);

            // Добавляем чат в базу данных
            await dbContext.Chats.AddAsync(chat);

            // Сохраняем изменения
            await dbContext.SaveChangesAsync();

            // Возвращаем id созданного чата
            return chat.ChatId;
        }

        public async Task<User> AddUserToChatAsync(int userId, int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            // Находим чат по id
            Chat? chat = await dbContext.Chats.FirstOrDefaultAsync(c => c.ChatId == chatId);
            // Находим пользователя по id
            User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            // Пользователь или чат не существует - бросаем ошибку
            if (chat == null || user == null)
                throw new NullReferenceException("Chat or user doesn't exist");

            // Добавляем пользователя в список участников
            chat.Members.Add(user);

            // Сохранение изменений
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUserFromChatAsync(int userId, int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            // Находим чат по id
            Chat? chat = await dbContext.Chats.Include(c => c.Members).FirstOrDefaultAsync(c => c.ChatId == chatId);
            // Находим пользователя в списке участников чата
            User? user = chat?.Members.Find(u => u.UserId == userId);

            // Пользователь или чат не существует - бросаем ошибку
            if (chat == null || user == null)
                throw new NullReferenceException("Chat or user doesn't exist");

            // Пользователь и чат существуют - удаляем пользователя из списка участников чата
            chat.Members.Remove(user);

            // Если удалённый пользователь был последним (в чате осталось 0 пользователей) - удаляем сам чат из базы данных
            if (chat.Members.Count <= 0)
                dbContext.Chats.Remove(chat);

            // Сохранение изменений
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsChatContainsMember(int userId, int chatId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            Chat? chat = await dbContext.Chats.Include(c => c.Members).FirstOrDefaultAsync(c => c.ChatId == chatId);
            if (chat == null)
                throw new NullReferenceException("Chat doesn't exist");

            return chat.Members
                .ToList()
                .Any(member => member.UserId == userId);
        }
    }
}
