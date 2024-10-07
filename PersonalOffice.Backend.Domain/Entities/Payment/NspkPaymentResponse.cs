namespace PersonalOffice.Backend.Domain.Entities.Payment
{
    /// <summary>
    /// Контракт на получение данных о пополнении
    /// </summary>
    public class NspkPaymentResponse
    {
        /// <summary>
        /// Идентификатор 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Содержимое файл или qr code
        /// </summary>
        public string? Payload { get; set; }
    }
}
