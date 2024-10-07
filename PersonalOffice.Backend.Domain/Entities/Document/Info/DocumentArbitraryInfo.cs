using System.Xml.Serialization;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Info.Base;

namespace PersonalOffice.Backend.Domain.Entities.Document.Info
{
    /// <summary>
    /// Информция об ином документе
    /// </summary>
    [Serializable]
    [XmlRoot("DocArbitraryInfo")]
    public class DocumentArbitraryInfo: DocumentBaseInfo
    {
        /// <summary>
        /// Название
        /// </summary>
        public required string DocName { get; set; }
        /// <summary>
        /// Коментарий
        /// </summary>
        public required string DocComment { get; set; }
        /// <summary>
        /// Хэш прикрепленных файлов
        /// </summary>
        public ICollection<DocElement<string, string>> FilesHash { get; set; } = [];

    }
}
