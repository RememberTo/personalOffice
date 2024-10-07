using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.File
{
    /// <summary>
    /// Модель для информации о файла из MinIO
    /// </summary>
    public class FileInfoMinIOResult
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.FileInfoMinIOResult, MessageDataTypes";
        /// <summary>
        /// Название файла
        /// </summary>
        public required string FileName {  get; set; }
        /// <summary>
        /// Уникальный тэг файла
        /// </summary>
        public required string ETag { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
        public ulong Size { get; set; }
        /// <summary>
        /// Метаданные файла
        /// </summary>
        public required IDictionary<string, string> MetaData;
    }
}
