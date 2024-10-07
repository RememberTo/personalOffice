using MediatR;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetCustomReport
{
    /// <summary>
    /// Контракт на получение созданного отчета
    /// </summary>
    public class GetCustomReportQuery : IRequest<CustomReportVm?>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
        /// <summary>
        /// Идентификатор созданного отчета
        /// </summary>
        public required string ReportId { get; set; }
    }
}
