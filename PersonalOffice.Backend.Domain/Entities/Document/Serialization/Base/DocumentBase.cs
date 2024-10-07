using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entities.Document.Serialization.Base
{
    /// <summary>
    /// Базовый класс документа для сериализации
    /// </summary>
    [Serializable]
    [XmlRoot("DocBaseClass")]
    public class DocumentBase
    {
        /// <summary>
        /// Версия документа
        /// </summary>
        [XmlElement(Order = 0)]
        public string Version { get; set; } = "1.0.0.0";

        /// <summary>
        /// Идентификатор договора
        /// </summary>
        [XmlElement(Order = 1)]
        public int ContractID { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        [XmlElement(Order = 2)]
        public string? Code { get; set; }
        /// <summary>
        /// Тип документа
        /// </summary>
        [XmlElement(Order = 3)]
        public int TypeID { get; set; }
        /// <summary>
        /// Идентификатор связанного документа
        /// </summary>
        [XmlElement(Order = 4)]
        public int ParentID { get; set; }
        /// <summary>
        /// Время
        /// </summary>
        [XmlElement(Order = 5)]
        public DateTime Date { get; set; }
    }
}
