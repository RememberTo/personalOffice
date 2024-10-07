using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.File
{
    /// <summary>
    /// Контракт на запрос информации о файле из MinIO
    /// </summary>
    public class FileReadMinIORequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.FileReadMinIORequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Название области хранения
        /// </summary>
        public required string Bucket { get; set; }
    }
}
