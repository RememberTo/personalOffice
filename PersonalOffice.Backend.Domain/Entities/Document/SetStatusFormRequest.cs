using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на уствновку статуса формы
    /// </summary>
    public class SetStatusFormRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SF_FormSetStatusRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocId {  get; set; }
        /// <summary>
        /// Устанавливаемый идентфиикатор статуса
        /// </summary>
        [JsonProperty("StatusID")]
        public int StatusId {  get; set; }
    }
}
