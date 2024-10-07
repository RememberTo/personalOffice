namespace PersonalOffice.Backend.Domain.Entites.File
{
    /// <summary>
    /// Модель ответа для файловых операций
    /// </summary>
    public class FileOperationResult
    {
        /// <summary>
        /// Тип операции
        /// </summary>
        public string? Operation { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Статус выполнения
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Статус файла
        /// </summary>
        public bool FileWritten { get; set; }
        /// <summary>
        /// Содержимое 
        /// </summary>
        public required byte[] Content { get; set; }
        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// Коментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
