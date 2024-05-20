using Microsoft.EntityFrameworkCore;
using System.Data;
using ZeraMessanger.DbContexts;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.MessageDto;

namespace ZeraMessanger.Services.Repositories.EfCoreRepositories
{
    public class MessagesRepository : IMessagesRepository
    {
        public async Task<int> AddMessageAsync(NewMessageData newMessageData, int authorId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            User? messageAuthor = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == authorId);
            if (messageAuthor == null)
                throw new NullReferenceException("User doesn't exist");

            Message? message = new Message
            {
                MessageText = newMessageData.MessageText,
                ChatId = newMessageData.ChatId,
                AuthorId = messageAuthor.UserId,
                AuthorName = messageAuthor.UserName
            };

            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();

            return message.MessageId;
        }
    }
}
