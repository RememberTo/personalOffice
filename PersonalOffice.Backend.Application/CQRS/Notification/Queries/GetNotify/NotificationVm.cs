namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify
{
    /// <summary>
    /// Модель представления уведомления
    /// </summary>
    public class NotificationVm
    {
        /// <summary>
        /// Идентификатор уведомления
        /// </summary>
        public required int NotifyID { get; set; }
        /// <summary>
        /// Тема уведолмения
        /// </summary>
        public required string Subject { get; set; }
        /// <summary>
        /// Содержимое уведомления
        /// </summary>
        public string? Body { get; set; }
        /// <summary>
        /// Дата и время отправки
        /// </summary>
        public required DateTime SentTime { get; set; }
        /// <summary>
        /// Прочитано ли уведомление
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Количество файлов в уведомлении
        /// </summary>
        public byte FileCount { get; set; }
    }
}
