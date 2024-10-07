using MediatR;
using PersonalOffice.Backend.Application.CQRS.File.Commands;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateArbitraryDocument
{
    /// <summary>
    /// Контракт на регистрацию произвольного документа
    /// </summary>
    public class CreateArbitraryDocumentCommand : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентфиикатор договора
        /// </summary>
        public int ContractId { get; set; }
        /// <summary>
        /// Название документа
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Комментарий к документу
        /// </summary>
        public required string Comment { get; set; } = string.Empty;
        /// <summary>
        /// Список загруженных файлов
        /// </summary>
        public required ICollection<UploadFile> UploadFiles { get; set; }
    }
}
