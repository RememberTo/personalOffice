namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocHash
{
    /// <summary>
    /// Модель представления хэша документа
    /// </summary>
    public class DocHashVm 
    {
        /// <summary>
        /// Хэш документа
        /// </summary>
        public required string Hash { get; set; }
    }
}
