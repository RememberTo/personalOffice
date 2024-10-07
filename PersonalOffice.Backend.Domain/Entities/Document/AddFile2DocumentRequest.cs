using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Контракт на добавления файла к документу
    /// </summary>
    public class AddFile2DocumentRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_AddFile2DocRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocId { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public required string FilePath { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public required string FileName { get; set; }
    }
}
