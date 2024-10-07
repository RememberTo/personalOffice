using PersonalOffice.Backend.Application.CQRS.Report.General;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetReports
{
    /// <summary>
    /// Модель представления списка отчетов
    /// </summary>
    public class ReportsVm
    {
        /// <summary>
        /// Общее количество отчетов
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// Список отчетов
        /// </summary>
        public required IEnumerable<ReportVm> Reports { get; set; }
    }
}
