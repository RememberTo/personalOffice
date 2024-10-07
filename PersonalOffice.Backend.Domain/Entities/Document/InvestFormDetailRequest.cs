using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на добавлении детализации формы
    /// </summary>
    public class InvestFormDetailRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_InvestFormDetailRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор формы
        /// </summary>
        [JsonProperty("FormID")]
        public int FormId {  get; set; }
        /// <summary>
        /// Идентфикатор поля
        /// </summary>
        [JsonProperty("FieldID")]
        public int FieldId { get; set; }
        /// <summary>
        /// Значение поля
        /// </summary>
        public required string Value { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
