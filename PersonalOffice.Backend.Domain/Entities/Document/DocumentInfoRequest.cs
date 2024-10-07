using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на получение документа
    /// </summary>
    public class DocumentInfoRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_InfoRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
        /// <summary>
        /// Идентфиикатор документа
        /// </summary>
        public required int DocId { get; set; }
    }
}
