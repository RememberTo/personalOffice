using System.Xml.Serialization;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Info.Base;

namespace PersonalOffice.Backend.Domain.Entities.Document.Info
{
    /// <summary>
    /// Информация о анкете инвест профиля
    /// </summary>
    [Serializable]
    [XmlRoot("InvestProfileAnketaInfo")]
    public class InvestProfileAnketaInfo : DocumentBaseInfo
    {
        /// <summary>
        /// Элементы инвестиционного профиля, различные определяющие инвест профиль 
        /// </summary>
        public ICollection<DocElement<string, string>> Elements { get; set; } = [];
    }
}
