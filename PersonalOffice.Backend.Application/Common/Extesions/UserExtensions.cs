using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.Common.Extesions
{
    /// <summary>
    /// Класс расширения для пользователя
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Добавление списка ролей пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service"></param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async static Task<User?> AddRoles(this User user, IUserService service, CancellationToken cancellationToken = default)
        {
            if (user == null) return null;

            await service.AddRolesAsync(user, cancellationToken);

            return user;
        }

        /// <summary>
        /// Добавление списка сертификатов
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service"></param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async static Task<User?> AddCertificates(this User user, IUserService service, CancellationToken cancellationToken = default)
        {
            if (user == null) return null;

            await service.AddCertificatesAsync(user, cancellationToken);

            return user;
        }

        /// <summary>
        /// Добавление списка блокировок
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service"></param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async static Task<User?> AddBans(this User user, IUserService service, CancellationToken cancellationToken = default)
        {
            if (user == null) return null;

            await service.AddBansAsync(user, cancellationToken);

            return user;
        }
    }
}
