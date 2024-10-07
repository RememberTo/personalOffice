using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetFilesById
{
    /// <summary>
    /// DTO для получения файлов из уведомления
    /// </summary>
    public class GetFilesQuery : IRequest<IEnumerable<NotifyFileVm>>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
        /// <summary>
        /// Идентификатор уведомления
        /// </summary>
        public required int NotifyId { get; set; }
    }
}
