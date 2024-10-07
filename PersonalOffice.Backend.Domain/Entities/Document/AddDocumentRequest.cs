using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на добавление документа
    /// </summary>
    public class AddDocumentRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_AddDocRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор связанного документа
        /// </summary>
        public int? ParentID { get; set; }
        /// <summary>
        /// Тип документа
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractID { get; set; }
        /// <summary>
        /// Хэш документа
        /// </summary>
        public required string Hash { get; set; }
        /// <summary>
        /// Путь к файлам
        /// </summary>
        public required string FilePath { get; set; }
        /// <summary>
        /// Навзание документа
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Содержимое документа
        /// </summary>
        public required string Content { get; set; }
    }
}
