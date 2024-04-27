using Microsoft.Data.SqlClient;
using SoftworkMessanger.Models;
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
        User? GetById(int userId);

        /// <summary>
        /// Получение конкретного пользователя по имени.
        /// </summary>
        /// <param name="userName">Имя пользователя, которогу нужно вернуть.</param>
        /// <returns>Пользователь с указанным <paramref name="userName"/>.</returns>
        User? GetByUserName(string userName);

        /// <summary>
        /// Получение одного пользователя из читателя данных SQL-запроса.
        /// </summary>
        /// <param name="dataReader">Читатель данных SQL-запроса.</param>
        /// <returns>Пользователь, полученный из читателя даных SQL-запроса.</returns>
        User GetUserFromReader(IDataReader dataReader);
    }
}
