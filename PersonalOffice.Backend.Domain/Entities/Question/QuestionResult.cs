namespace PersonalOffice.Backend.Domain.Entites.Question
{
    /// <summary>
    /// Результат выполнения операции для вопросов менеджеру
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuestionResult<T>
    {
        /// <summary>
        /// Результат выполнения операции
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public T? Data { get; set; }
        /// <summary>
        /// Комментарий 
        /// </summary>
        public string? Comment { get; set; }
    }
}
