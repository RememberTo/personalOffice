namespace PersonalOffice.Backend.Application.CQRS.Report.General
{
    /// <summary>
    /// Модель представления отчета
    /// </summary>
    public class ReportVm
    {
        /// <summary>
        /// Номер
        /// </summary>
        public long RowN { get; set; }
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Дата отчета
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public string? File { get; set; }
        /// <summary>
        /// Название файла при скачивании
        /// </summary>
        public string? FileExt { get; set; }
        /// <summary>
        /// Период отчета
        /// </summary>
        public string? PeriodName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Place { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PlaceID { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Договор
        /// </summary>
        public string? Contract { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ContractBranch { get; set; }
        /// <summary>
        /// подписаный отчет
        /// </summary>
        public bool IsSigned { get; set; }
        /// <summary>
        /// Новый отчет
        /// </summary>
        public bool IsNew { get; set; }
    }
}
