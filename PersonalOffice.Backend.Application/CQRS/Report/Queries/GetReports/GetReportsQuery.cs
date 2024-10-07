using MediatR;
using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetReports
{
    /// <summary>
    /// Контракт на получение списка отчетов
    /// </summary>
    public class GetReportsQuery : IRequest<ReportsVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.RP_ReportListRequest, MessageDataTypes";
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("ID")]
        public int UserId { get; set; }
        /// <summary>
        /// Количество страниц
        /// </summary>
        [JsonProperty("Page")]
        public int PageNum { get; set; }
        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        [JsonProperty("PageSize")]
        public int Count { get; set; }
        /// <summary>
        /// Период
        /// </summary>
        public int PeriodID { get; set; } = 0;
        /// <summary>
        /// Отчеты с подписью
        /// </summary>
        public bool WithSign { get; set; } = true;
    }
}
