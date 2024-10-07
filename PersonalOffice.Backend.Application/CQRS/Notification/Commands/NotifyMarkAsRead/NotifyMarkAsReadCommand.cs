using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotifyById
{
    /// <summary>
    /// Контракт на обновление статуса уведомления
    /// </summary>
    public class NotifyMarkAsReadCommand : IRequest<NotificationVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PONotifyMarkAsReadRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
        /// <summary>
        /// Идентификатор уведомления
        /// </summary>
        public required int NotifyId { get; set; }
        /// <summary>
        /// Пометить уведомление
        /// </summary>
        [JsonIgnore]
        public bool MarkNotify { get; set; } = true;
    }
}
