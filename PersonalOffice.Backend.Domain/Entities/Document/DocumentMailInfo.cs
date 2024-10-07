namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Модель для отправки документа почтовым сообщением
    /// </summary>
    public class DocumentMailInfo
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public int DocumentId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
        /// <summary>
        /// Идентификатор формы документа
        /// </summary>
        public int FormId { get; set; }
    }
}
