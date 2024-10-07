namespace PersonalOffice.Backend.Domain.Entites.SMS
{
    /// <summary>
    /// Результат отправки смс сообщения
    /// </summary>
    public class SmsOperationResult
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Результат выполнения операции
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Сообщение о результате
        /// </summary>
        public string? Message { get; set; }
    }
}
