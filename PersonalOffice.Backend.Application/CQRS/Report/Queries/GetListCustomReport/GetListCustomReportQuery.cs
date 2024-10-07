using MediatR;
using PersonalOffice.Backend.Application.CQRS.Report.General;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetListCustomReport
{
    /// <summary>
    /// Контракт на получение списка созданныхз отчетов
    /// </summary>
    public class GetListCustomReportQuery : IRequest<IEnumerable<CustomReportVm>>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
    }
}
