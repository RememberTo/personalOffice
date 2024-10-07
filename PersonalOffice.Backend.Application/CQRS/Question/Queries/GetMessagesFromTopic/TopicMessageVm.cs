namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic
{
    /// <summary>
    /// Модель представления для сообщений из топика
    /// </summary>
    public class TopicMessageVm
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public int MessageID { get; set; }
        /// <summary>
        /// Идентификатор отправителя сообщения
        /// </summary>
        public int PersonID { get; set; }
        /// <summary>
        /// Имя отправителя сообщения
        /// </summary>
        public string? PersonName { get; set; }
        /// <summary>
        /// Дата отправки
        /// </summary>
        public DateTime SentTime { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string? MessageText { get; set; }
        /// <summary>
        /// Прочитано сообщение или нет
        /// </summary>
        public bool IsRead { get; set; }


    }
}

