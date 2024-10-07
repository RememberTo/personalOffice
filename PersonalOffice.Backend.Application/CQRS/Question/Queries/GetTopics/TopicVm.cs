namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics
{
    /// <summary>
    /// Модель представления топика
    /// </summary>
    public class TopicVm
    {
        /// <summary>
        /// Идентификатор топика
        /// </summary>
        public int TopicID { get; set; }
        /// <summary>
        /// Дата и время последнего сообщения
        /// </summary>
        public DateTime LastMessageDate { get; set; }
        /// <summary>
        /// Заголовок
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Контракты
        /// </summary>
        public string? Contracts { get; set; }
        /// <summary>
        /// Менеджер
        /// </summary>
        public string? Manager { get; set; }
        /// <summary>
        /// Количество сообщений
        /// </summary>
        public int MessageCount { get; set; }
        /// <summary>
        /// Количество не прочитанных сообщений
        /// </summary>
        public int UnreadCount { get; set; }
        /// <summary>
        /// Закрыт топик да или нет
        /// </summary>
        public bool IsClosed { get; set; }
        /// <summary>
        /// Первое сообщение
        /// </summary>
        public string? FirstMessage { get; set; }
    }
}
