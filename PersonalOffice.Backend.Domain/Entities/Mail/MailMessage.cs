using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Mail
{
    /// <summary>
    /// Почтовое сообщение
    /// </summary>
    public class MailMessage
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.MailMessage, MessageDataTypes";
        /// <summary>
        /// Идентификатор почты
        /// </summary>
        [JsonProperty("MailID")]
        public int MailId { get; set; }
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public string? MessageId { get; set; }
        /// <summary>
        /// Список почтовых адресов кому отправить сообщение
        /// </summary>
        public ICollection<string> To { get; set; } = [];
        /// <summary>
        /// Список почтовых адресов для установки в копию
        /// </summary>
        public ICollection<string> CC { get; set; } = [];
        /// <summary>
        /// Список почтовых адресов для установки в скрытую копию
        /// </summary>
        public ICollection<string> BCC { get; set; } = [];
        /// <summary>
        /// Сообщение отправившее сообщение
        /// </summary>
        public required string Application { get; set; }
        /// <summary>
        /// От кого отправлено сообщение
        /// </summary>
        public string? From { get; set; }
        /// <summary>
        /// Почтовый адрес на который должен быть отправлен ответ
        /// </summary>
        public string? ReplyTo { get; set; }
        /// <summary>
        /// Тема сообщения
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// Текстовое содержимое сообщения 
        /// </summary>
        public string? TextBody { get; set; }
        /// <summary>
        /// Текстовое содержимое сообщения в виде HTML
        /// </summary>
        public string? HtmlBody { get; set; }
        /// <summary>
        /// Список прикрепленных файлов
        /// </summary>
        public ICollection<MailAttachment> Attachments { get; set; } = [];
    }
}
