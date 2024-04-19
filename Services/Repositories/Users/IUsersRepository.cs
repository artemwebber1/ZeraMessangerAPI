using SoftworkMessanger.Models;

namespace SoftworkMessanger.Services.Repositories.Users
{
    public interface IUsersRepository
    {
        User GetById(int userId);
        User GetByUserName(string userName);
    }
}
