using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Mail
{
    /// <summary>
    /// Контракт на отправку сообщения
    /// </summary>
    public class MailRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.MailSendRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор запроса
        /// </summary>
        public string RequestID { get; set; } = string.Empty;
        /// <summary>
        /// Отправляемое сообщение
        /// </summary>
        public required MailMessage Message { get; set; }
    }
}
