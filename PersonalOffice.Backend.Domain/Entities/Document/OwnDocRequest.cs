using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// 
    /// </summary>
    public class OwnDocRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_OwnDocRequest, MessageDataTypes";
        /// <summary>
        /// Идентфиикатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocId { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("UserID")]
        public int UserId { get; set; }
    }
}
