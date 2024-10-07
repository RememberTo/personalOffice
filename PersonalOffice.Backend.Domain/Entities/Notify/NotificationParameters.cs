using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entites.Notify
{
    /// <summary>
    /// Параметры отправки уведомления
    /// </summary>
    public class NotificationParameters
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SendNotificationRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Заголовок
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string? Message { get; set; }
    }
}
