using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetCustomReportFile
{
    /// <summary>
    /// Контракт на получение файла созданного отчета
    /// </summary>
    public class GetCustomReportFileQuery : IRequest<FileVm>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор созданного отчета (GUID)
        /// </summary>
        public required string ReportId { get; set; }
        /// <summary>
        /// Навзание файла
        /// </summary>
        public string? FileName { get; set; }
    }
}
