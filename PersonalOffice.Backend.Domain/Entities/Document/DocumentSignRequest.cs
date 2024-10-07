using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на подписание документа во внутренней системе
    /// </summary>
    public class DocumentSignRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_SignRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocId { get; set; }
        /// <summary>
        /// Идентфикатор пользователя
        /// </summary>
        [JsonProperty("UserID")]
        public int UserId { get; set; }
        /// <summary>
        /// Код подписи
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Путь к файлу подписи
        /// </summary>
        public required string FilePath { get; set; }
    }
}
