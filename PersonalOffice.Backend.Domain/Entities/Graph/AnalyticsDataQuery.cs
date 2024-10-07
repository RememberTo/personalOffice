using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Domain.Entities.Graph
{
    /// <summary>
    /// Контракт на получение данных для графика
    /// </summary>
    public class AnalyticsDataQuery 
    {
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
        public int GraphId { get; set; }
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
        public Currency CurrencyId { get; set; }
    }
}
