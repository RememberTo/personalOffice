namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopicById
{
    /// <summary>
    /// Модель представления информации о топика
    /// </summary>
    public class TopicInfoVm
    {
        /// <summary>
        /// Идентфииактор топика
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Заголовок
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// Дата и время создания топика
        /// </summary>
        public DateTime Date { get; set; }
    }
}
