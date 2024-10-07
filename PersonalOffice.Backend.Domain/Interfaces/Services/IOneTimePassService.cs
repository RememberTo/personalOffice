using PersonalOffice.Backend.Domain.Entities.OneTimePass;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис одноразового пароля
    /// </summary>
    public interface IOneTimePassService
    {
        /// <summary>
        /// Проверяет одноращовый пароль
        /// </summary>
        /// <param name="command">Данные для северки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task CheckOtpAsync(CheckOtp command, CancellationToken cancellationToken = default);
    }
}
