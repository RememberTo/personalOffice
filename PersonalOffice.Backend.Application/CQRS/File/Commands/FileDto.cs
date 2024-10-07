namespace PersonalOffice.Backend.Application.CQRS.File.Commands
{
    /// <summary>
    /// Модель загруженного файла
    /// </summary>
    public class UploadFile 
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string Name => Path.GetFileNameWithoutExtension(FileName);
        /// <summary>
        /// Название файла с расширением
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
        public required long Length { get; set; }
        /// <summary>
        /// Поток, содержащий сам файл
        /// </summary>
        public required Stream Stream { get; set; }
    }
}
