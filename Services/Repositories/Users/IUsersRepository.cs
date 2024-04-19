using SoftworkMessanger.Models;

namespace SoftworkMessanger.Services.Repositories.Users
{
    /// <summary>
    /// Репозиторий для работы с таблицей пользователей.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Получение конкретного пользователя по Id.
        /// </summary>
        /// <param name="userId">Id пользователя, которого нужно вернуть.</param>
        /// <returns>Пользователь с указанным <paramref name="userId"/>.</returns>
        User? GetById(int userId);

        /// <summary>
        /// Получение конкретного пользователя по имени.
        /// </summary>
        /// <param name="userName">Имя пользователя, которогу нужно вернуть.</param>
        /// <returns>Пользователь с указанным <paramref name="userName"/>.</returns>
        User? GetByUserName(string userName);
    }
}
