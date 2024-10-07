namespace PersonalOffice.Backend.Domain.Entites.Report
{
    /// <summary>
    /// Модель для передачи данных о создаваемом отчете
    /// </summary>
    public class CustomReport
    {
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public required string ReportID { get; set; }
        /// <summary>
        /// Результат формирования отчета
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Формат отчета
        /// </summary>
        public required string Format { get; set; }
        /// <summary>
        /// Содержимое отчета в бинарном виде
        /// </summary>
        public required byte[] Content { get; set; }
        /// <summary>
        /// Коментарий к отчету
        /// </summary>
        public string? Comment { get; set; }
    }
}
