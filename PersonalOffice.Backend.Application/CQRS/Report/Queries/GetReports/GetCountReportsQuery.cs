using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetReports
{
    /// <summary>
    /// Контракт на получение количества отчетов
    /// </summary>
    internal class GetCountReportsQuery
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.RP_CountRequest, MessageDataTypes";

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("ID")]
        public int UserId { get; set; }
        /// <summary>
        /// Период 
        /// </summary>
        public int PeriodID { get; set; } = 0;
    }
}
