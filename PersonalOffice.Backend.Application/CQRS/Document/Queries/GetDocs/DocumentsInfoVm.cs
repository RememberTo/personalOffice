namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs
{
    /// <summary>
    /// Модель представления документов
    /// </summary>
    public class DocumentsInfoVm
    {
        /// <summary>
        /// Общее количество документов
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Информация о последней инвестиционной анкете
        /// </summary>
        public required string LastAgreedInvestmentQuestionnaireInfo { get; set; }
        /// <summary>
        /// Список документов
        /// </summary>
        public required ICollection<DocumentInfoVm> Docs { get; set; }
    }
}
