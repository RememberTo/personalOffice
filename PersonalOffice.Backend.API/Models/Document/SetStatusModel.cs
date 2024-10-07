namespace PersonalOffice.Backend.API.Models.Document
{
    /// <summary>
    /// Модель смены статуса для документа
    /// </summary>
    public class SetStatusModel
    {
        /// <summary>
        /// Идентфиикатор статуса
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Комментарий к статусу
        /// </summary>
        public string? CommentStatus { get; set; }
    }
}
