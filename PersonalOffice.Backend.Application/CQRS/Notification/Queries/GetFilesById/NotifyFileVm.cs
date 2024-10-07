namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetFilesById
{
    /// <summary>
    /// Модель представления файлов из уведомления
    /// </summary>
    public class NotifyFileVm
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public required string ID { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public string? Name { get; set; }
    }
}
