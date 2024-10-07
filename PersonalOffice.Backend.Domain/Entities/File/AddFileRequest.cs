using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.File
{
    /// <summary>
    /// Контракт на добавление файла
    /// </summary>
    public class AddFileRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.AddFileRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int EntityID { get; set; }
        /// <summary>
        /// Наименование файла
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Расшиерние файла
        /// </summary>
        public string? FileExtension { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public required byte[] FileContent { get; set; }
    }
}
