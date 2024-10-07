namespace PersonalOffice.Backend.Domain.Entites.SQL
{
    /// <summary>
    /// Результат выполнения sql операции
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SQLOperationResult<T>
    {
        /// <summary>
        /// Статус выполнения
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Данные выполнения операции
        /// </summary>
        public T? ReturnValue { get; set; }
        /// <summary>
        /// Сообщение о результате
        /// </summary>
        public string? Message { get; set; }
    }
}
