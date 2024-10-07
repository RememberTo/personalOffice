using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на добавление формы 
    /// </summary>
    public class AddFormRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_AddFormRequest, MessageDataTypes";
        /// <summary>
        /// Идентфиикатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        [JsonProperty("ContractID")]
        public int ContractId { get; set; }
    }
}
