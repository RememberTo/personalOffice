using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entites.File
{
    /// <summary>
    /// Контракт для передачи данных о файле
    /// </summary>
    public class FileOperationParameters
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.FileOperationParam, MessageDataTypes";
        /// <summary>
        /// Навзание файла
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Переименовать файл
        /// </summary>
        public string? NameOut { get; set; }
        /// <summary>
        /// Перезапись
        /// </summary>
        public bool Overwrite { get; set; } = false;
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[]? Content { get; set; }
        /// <summary>
        /// Стартовый индекс чтения массива байт (content)
        /// </summary>
        public int StartIndex { get; set; } = 0;
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; } = 0;
    }
}
