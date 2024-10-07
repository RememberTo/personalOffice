namespace PersonalOffice.Backend.Application.CQRS.File.Queries
{
    /// <summary>
    /// Модель представления файла
    /// </summary>
    public class FileVm
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public required byte[] Content { get; set; }
        /// <summary>
        /// Тип содержимого
        /// </summary>
        public required string ContentType { get; set; }
    }
}
