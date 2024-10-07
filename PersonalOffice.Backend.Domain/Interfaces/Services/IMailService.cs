using PersonalOffice.Backend.Domain.Entities.Mail;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для отправки почтовых сообщений
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Отправляет почтовое письмо
        /// </summary>
        /// <param name="message">модель сообщения</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task SendAsync(MailMessage message, CancellationToken cancellationToken = default);
        /// <summary>
        /// Отправка сообщения с оберткой try catch
        /// </summary>
        /// <param name="message">модель сообщения</param>
        /// <param name="mailResponse">ответ</param>
        /// <returns></returns>
        public bool TrySend(MailMessage message, out MailResponse? mailResponse);
    }
}
