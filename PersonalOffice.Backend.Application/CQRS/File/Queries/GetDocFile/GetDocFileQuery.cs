using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetDocFile
{
    /// <summary>
    /// Контракт на получение файла документа по идентификатору
    /// </summary>
    public class GetDocFileQuery : IRequest<FileVm>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public int FileId { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public string? FileName { get; set; }
    }
}
