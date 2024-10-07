namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify
{
    /// <summary>
    /// Модель представление уведомлений
    /// </summary>
    public class NotificationsVm
    {
        /// <summary>
        /// Количество непрочитанных уведомлений
        /// </summary>
        public long UnreadCount { get; set; }
        /// <summary>
        /// Список уведомлений
        /// </summary>
        public required IEnumerable<NotificationVm> Notifications { get; set; }
    }
}