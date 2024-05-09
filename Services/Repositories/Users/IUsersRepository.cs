using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
using SoftworkMessanger.Models.Dto.UserDto;
using System.Data;

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
        Task<User?> GetByIdAsync(int userId);

        /// <summary>
        /// Получение конкретного пользователя по электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя, которого нужно вернуть.</param>
        /// <returns>Пользователь с указанным <paramref name="email"/>.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Получение одного пользователя из читателя данных SQL-запроса.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Пользователь, полученный из читателя даных SQL-запроса.</returns>
        User GetUserFromReader(IDataReader dataReader);

        /// <summary>
        /// Создание нового пользователя в базе даных.
        /// </summary>
        /// <param name="name">Имя нового пользователя.</param>
        /// <param name="hashedPassword">Захэшированный пароль пользователя.</param>
        /// <param name="email">Электронная почта нового пользователя.</param>
        Task AddUserAsync(string name, string hashedPassword, string email);

        /// <summary>
        /// Проверяет, существует ли пользователь с заданной электронной почте в базе данных.
        /// </summary>
        /// <param name="email">Электронная почта.</param>
        /// <returns>True, если пользователь с заданной электронной почтой существует, иначе false.</returns>
        Task<bool> IsUserExistsWithEmail(string email);
    }
}
