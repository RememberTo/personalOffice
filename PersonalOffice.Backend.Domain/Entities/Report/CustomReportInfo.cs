namespace PersonalOffice.Backend.Domain.Entities.Report
{
    /// <summary>
    /// Информация о создаваемом отчете
    /// </summary>
    public class CustomReportInfo
    {
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
        /// <summary>
        /// Тип отчета
        /// </summary>
        public required string ReportType { get; set; }
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public required string ReportId { get; set; }
        /// <summary>
        /// Номер договора
        /// </summary>
        public string? Prefix { get; set; }
        /// <summary>
        /// Формат отчета
        /// </summary>
        public string? Format { get; set; }
        /// <summary>
        /// Начальная дата отчета
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// Конечная дата отчета
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Навзание файла
        /// </summary>
        public string? Filename { get; set; }
    }
}
