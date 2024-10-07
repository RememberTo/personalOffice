using MediatR;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Report.Commands.UpdateReport
{
    /// <summary>
    /// Контракт на обновление статуса отчета
    /// </summary>
    public class UpdateReportCommand : IRequest <IResult>
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
        /// Сброс статуса
        /// </summary>
        public bool IsResetStatus { get; set; }
    }
}
