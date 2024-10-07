using System.Xml.Serialization;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Serialization.Base;

namespace PersonalOffice.Backend.Domain.Entities.Document.Serialization
{
    /// <summary>
    /// Модель для сериализации анкеты инвестиционного профиля
    /// </summary>
    [Serializable]
    [XmlRoot("DocInvestProfileClass")]
    public class DocumentInvestProfile : DocumentBase
    {
        /// <summary>
        /// Содержимое документа
        /// </summary>
        [XmlElement(Order = 10)]
        public List<DocElement<int, string>> Content { get; set; } = [];
        /// <summary>
        /// Установка версии
        /// </summary>
        public DocumentInvestProfile() { Version = "1.0.1.0"; }
    }
}
