using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Entities.SQL;
using PersonalOffice.Backend.Domain.Entities.User;
using System.Security.Cryptography.X509Certificates;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис предоставляющий взаимодействие с пользователем
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получение номера телефона
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<string> GetPhoneByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает возможнгость общения с инвест консультантом
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<bool> CanContactInvestmentConsultantAsync(int userId, CancellationToken cancellationToken);


        /// <summary>
        /// Возвращает общую стоимость всех портфелей, (ЦБ и денег)
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<decimal> GetTotalPortfolioValueAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Проверка на доступ к топику для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="topicId">Идентификатор топика</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        /// 
        public Task CheckUserTopicAccessAsync(int userId, int topicId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверка на доступ к списку файлов уведомления для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="notifyId">Идентификатор уведомления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task CheckUserNotifyFilesAccessAsync(int userId, int notifyId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение DocNum по contract id
        /// </summary>
        /// <param name="contractId">идентификатор контракта</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<string> GetDocNumAsync(int contractId, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение общей информации по пользователю
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<User> GetGeneralUserInfoAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление списка сертификатов
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task AddCertificatesAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление списка блокировок
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task AddBansAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление списка ролей пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task AddRolesAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение логина пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<string> GetUserLoginAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменение ролей у пользователя
        /// </summary>
        /// <param name="changeRoleRequest">Параметры смены роли</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task ChangeRoleAsync(ChangeRoleRequest changeRoleRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение профиля пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<UserProfile> GetUserProfileAsync(int userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получение персонального кода пользователя
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<string> GetPersonCodeAsync(int contractId, CancellationToken cancellationToken = default);
    }
}
