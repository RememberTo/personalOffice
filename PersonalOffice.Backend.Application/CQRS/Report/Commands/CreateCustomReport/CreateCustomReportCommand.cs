using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Domain.Entites.Report;

namespace PersonalOffice.Backend.Application.CQRS.Report.Commands.CreateCustomReport
{
    /// <summary>
    /// Модель для создания отчета c заданными параметрами
    /// </summary>
    public class CreateCustomReportCommand : IRequest<CustomReportVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.ClientReportRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public required int UserId {  get; set; }
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public required string ReportID { get; set; }
        /// <summary>
        /// Идентификатор контракта пользователя
        /// </summary>
        public int ContractID { get; set; }
        /// <summary>
        /// Начальная дата отчета
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// Конечная дата отчета
        /// </summary>
        public required DateTime EndDate { get; set; }
        /// <summary>
        /// Тип отчета
        /// </summary>
        public required string ReportType { get; set; }
        /// <summary>
        /// Наименование типа отчета
        /// </summary>
        public required string ReportTypeName { get; set; }
        /// <summary>
        /// Тип отчета, указывается формат
        /// </summary>
        public required string ReportFormat { get; set; }
        /// <summary>
        /// Язык отчета цифрой
        /// </summary>
        public required string Language { get; set; }
        /// <summary>
        /// Валюта отчета
        /// </summary>
        [JsonProperty("AssetIDCur")]
        public required string Currency { get; set; }
        /// <summary>
        /// Тип цены для отчета (рыночная или по последним сделкам)
        /// </summary>
        public required string PriceType { get; set; }
    }
}
