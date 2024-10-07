using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Contract
{
    /// <summary>
    /// Контракт на получение данных для договора
    /// </summary>
    public class ContractDataRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SF_ContractDataRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор графика
        /// Может использоваться для PersonId ContractId и т п
        /// </summary>
        [JsonProperty("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Начальная дата данных графика
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// Конечная дата данных графика
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Идентификатор валюты -> Data.Currency
        /// </summary>
        [JsonProperty("CurrencyID")]
        public int CurrencyId { get; set; } = 1;
    }
}
