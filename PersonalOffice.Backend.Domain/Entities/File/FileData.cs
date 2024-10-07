namespace PersonalOffice.Backend.Domain.Entities.File
{
    /// <summary>
    /// Модель файла
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// Название файла 
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public required byte[] Data { get; set; }
    }
}
