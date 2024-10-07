using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Mail
{
    /// <summary>
    /// Прикрепляемый файл к сообщению
    /// </summary>
    public class MailAttachment
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.MailAttachment, MessageDataTypes";
        /// <summary>
        /// Наименование файла
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Тип содержимого файла
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public required byte[] Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsLinked { get; set; }
    }
}
