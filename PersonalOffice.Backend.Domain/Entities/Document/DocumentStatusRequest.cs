using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на установку статуса документа
    /// </summary>
    public class DocumentStatusRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_StatusRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocId { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("UserID")]
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор устанавливаемого статуса
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Дополнительный комментарий
        /// </summary>
        public string? StatusComment { get; set; } = null;
    }
}
