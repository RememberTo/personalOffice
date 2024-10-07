namespace PersonalOffice.Backend.Domain.Entites.OneTimePass
{
    /// <summary>
    /// Результат отправки одноразового кода
    /// </summary>
    public class OtpOperationResult
    {
        /// <summary>
        /// Результат выполенения операции
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дополнительное сообщение
        /// </summary>
        public required string Message { get; set; }
    }
}
