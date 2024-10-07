using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetReportFile
{
    /// <summary>
    /// Контракт на получение файла отчета по идентификатору
    /// </summary>
    public class GetReportFileQuery : IRequest<FileVm>
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
        /// <summary>
        /// Файл подписи
        /// </summary>
        public bool IsSignFile { get; set; } = false;
    }
}
