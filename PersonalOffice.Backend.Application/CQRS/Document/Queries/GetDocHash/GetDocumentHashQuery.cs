using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocHash
{
    /// <summary>
    /// Контракт на получение хэша документа
    /// </summary>
    public class GetDocumentHashQuery : IRequest<DocHashVm>
    {
        /// <summary>
        /// Идентфиикатор пользователя
        /// </summary>
        public int UserId {  get; set; }
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public int DocumentId { get; set; }
    }
}
