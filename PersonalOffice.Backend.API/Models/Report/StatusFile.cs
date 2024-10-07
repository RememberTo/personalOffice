namespace PersonalOffice.Backend.API.Models.Report
{
    /// <summary>
    /// Модель для частичного обновления статуса файла
    /// </summary>
    public class StatusFile
    {
        /// <summary>
        /// Сбросить статус нового файла
        /// </summary>
        public bool IsResetStatus { get; set; }
    }
}