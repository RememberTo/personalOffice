using System.Xml.Serialization;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Info.Base;

namespace PersonalOffice.Backend.Domain.Entities.Document.Info
{
    /// <summary>
    /// Информация о инвест справке
    /// </summary>
    [Serializable]
    [XmlRoot("InvestProfileSpravkaInfo")]
    public class InvestProfileSparvkaInfo : DocumentBaseInfo
    {
        /// <summary>
        /// Файлы инвестиционных српавок
        /// </summary>
        public ICollection<DocumentFileInfo> Files { get; set; } = [];
    }
}
