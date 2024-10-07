namespace PersonalOffice.Backend.Application.CQRS.Report.General
{
    /// <summary>
    /// Модель представления отчета
    /// </summary>
    public class CustomReportVm
    {
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public required string ReportId { get; set; }
        /// <summary>
        /// Название отчета
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Название файла отчета
        /// </summary>
        public string? FileName { get; set; }
    }
}
