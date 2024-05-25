using Microsoft.EntityFrameworkCore;
using ZeraMessanger.DbContexts;
using ZeraMessanger.Models;
using ZeraMessanger.Models.Dto.UserDto;
using ZeraMessanger.Services.Authentification;

namespace ZeraMessanger.Services.Repositories.EfCoreRepositories
{
    public class UsersRepository : IUsersRepository
    {
        public UsersRepository(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        private readonly IPasswordHasher _passwordHasher;

        public async Task<User?> GetByIdAsync(int userId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            User? user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            User? user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserEmail == email);
            return user;
        }

        public async Task AddUserAsync(string name, string hashedPassword, string email)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            User user = new User
            {
                UserName = name,
                UserPassword = hashedPassword,
                UserEmail = email
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserExistsWithEmail(string email)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();
            return await dbContext.Users.AsNoTracking().AnyAsync(u => u.UserEmail == email);
        }

        public async Task UpdateUserAsync(UserUpdateData updateData, int userId)
        {
            using ZeraDbContext dbContext = new ZeraDbContext();

            User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                throw new NullReferenceException("User to update is null");

            user.UserName = updateData.UserName;
            user.UserPassword = _passwordHasher.Generate(updateData.UserPassword);
            user.UserEmail = updateData.UserEmail;

            await dbContext.SaveChangesAsync();
        }
    }
}
