using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Entites.Pagination;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify
{
    /// <summary>
    /// DTO для выборки уведомлений пользователя
    /// </summary>
    public class GetNotifyQuery : IRequest<NotificationsVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PONotifyHistory4UserRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Количество уведомлений, 0 - все уведомления
        /// </summary>
        public required PageConfiguration PageConfig { get; set; }
    }
}
