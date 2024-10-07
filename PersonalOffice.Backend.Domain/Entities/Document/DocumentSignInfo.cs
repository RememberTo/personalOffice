using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Domain.Entities.Document
{
    /// <summary>
    /// Информация для подписания документа
    /// </summary>
    public class DocumentSignInfo
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public required int DocId { get; set; }
        /// <summary>
        /// Тип подписи
        /// </summary>
        public required SignType SignType { get; set; }
        /// <summary>
        /// Подпись
        /// </summary>
        public required string Signature { get; set; }
        /// <summary>
        /// Подпись
        /// </summary>
        public required string DocFilePath { get; set; }
    }
}
