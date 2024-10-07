using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest
{
    /// <summary>
    /// Контракт на запись файла
    /// </summary>
    public class FileOperationCommand
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.FileOperationParam, MessageDataTypes";
        /// <summary>
        /// Название файла
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Перезапись
        /// </summary>
        public bool Overwrite { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public required byte[] Content { get; set; }
    }
}
