using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Payment
{
    /// <summary>
    /// Контракт на создание пополнения
    /// </summary>
    public class PaymentByDetailsRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PaymentByDetailsRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор Догоовра
        /// </summary>
        public required string ContractID { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Бик банка
        /// </summary>
        public required string BIC { get; set; }
        /// <summary>
        /// Название банка
        /// </summary>
        public required string BankName { get; set; }
        /// <summary>
        ///  Корреспонденский счет
        /// </summary>
        public required string CorrespAcc { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        public required string PayeeINN { get; set; }
        /// <summary>
        /// КПП
        /// </summary>
        public required string KPP { get; set; }
        /// <summary>
        /// Счет персоны
        /// </summary>
        public required string PersonalAcc { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public required string Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public required string Purpose { get; set; }
        /// <summary>
        /// Сформированный QR code
        /// </summary>
        public required string QrCode { get; set; }
    }
}
