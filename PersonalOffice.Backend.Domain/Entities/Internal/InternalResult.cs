namespace PersonalOffice.Backend.Domain.Entites.Internal
{
    /// <summary>
    /// Результат внутреннего микросервиса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InternalResult<T>
    {
        /// <summary>
        /// Содержимое
        /// </summary>
        public T? Value { get; set; }
        /// <summary>
        /// Статус выполения
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}