using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Payment
{
    /// <summary>
    /// Контракт на получение данных для пополнения
    /// </summary>
    public class NspkPaymentRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.NspkPaymentRequest, MessageDataTypes";
        /// <summary>
        /// Идентфиикатор договора
        /// </summary>
        public int ContractID { get; set; }
        /// <summary>
        /// Идентификатор портфеля
        /// </summary>
        public int PortfolioID { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Оператор
        /// </summary>
        public NspkOperator Operator { get; set; }
        /// <summary>
        /// Содержимое
        /// </summary>
        public string? Body { get; set; }
    }
}
