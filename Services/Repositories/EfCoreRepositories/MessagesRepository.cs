using Microsoft.EntityFrameworkCore;
using ZeraMessanger.DbContexts;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;

namespace ZeraMessanger.Services.Repositories.EfCoreRepositories
{
    public class MessagesRepository : IMessagesRepository
    {
        public async Task<Message> AddMessageAsync(NewMessageData newMessageData, int? authorId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            User? messageAuthor = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == authorId);

            Message message = new Message
            {
                MessageText = newMessageData.MessageText,
                ChatId = newMessageData.ChatId,
                AuthorId = authorId,
                AuthorName = messageAuthor?.UserName
            };

            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();

            return message;
        }
    }
}
