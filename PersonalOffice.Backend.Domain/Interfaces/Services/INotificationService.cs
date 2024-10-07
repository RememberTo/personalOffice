using PersonalOffice.Backend.Domain.Entites.Notify;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис отвечающий за работу с уведомлениями
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Отправка уведомления пользователю
        /// </summary>
        /// <param name="notificationParameters">Параметры отправки</param>
        /// <returns></returns>
        public Task SendNotificationMessageAsync(NotificationParameters notificationParameters);
    }
}
