using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entites.Document
{
    /// <summary>
    /// Контракт на закрепление файла за документом
    /// </summary>
    public class DocumentFilePathRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_FilePathRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocumentId { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public required string FilePath {  get; set; }
    }
}
