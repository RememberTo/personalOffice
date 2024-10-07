using MediatR;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.DeleteDocument
{
    /// <summary>
    /// Контракт на удаление документа
    /// </summary>
    public class DeleteDocumentCommand : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public int DocumentId { get; set; }
    }
}
