using PersonalOffice.Backend.Domain.Entities.Document.Info.Base;

namespace PersonalOffice.Backend.Domain.Entities.Document.Elements
{
    /// <summary>
    /// Модель для сериализации запроса
    /// </summary>
    [Serializable]
    public class DocumentInfoDataTable : DocumentBaseInfo
    {
        /// <summary>
        /// Путь к файлу документа
        /// </summary>
        public required string FilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? BranchId { get; set; }
    }
}
