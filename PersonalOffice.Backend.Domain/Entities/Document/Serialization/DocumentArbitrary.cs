using System.Xml.Serialization;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Entities.Document.Serialization.Base;

namespace PersonalOffice.Backend.Domain.Entities.Document.Serialization
{

    /// <summary>
    /// Модель для сериализации произвольного документа
    /// </summary>
    [Serializable]
    [XmlRoot("DocArbitraryClass")]
    public class DocumentArbitrary : DocumentBase
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DocArbitraryClass, MessageDataTypes";
        
        /// <summary>
        /// Номер догоовра
        /// </summary>
        [XmlElement(Order = 8)]
        public required string DocNum { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [XmlElement(Order = 9)]
        public required string PersonName { get; set; }
        /// <summary>
        /// Идентфииктаор документа
        /// </summary>
        [XmlElement(Order = 10)]
        public int DocId { get; set; }
        /// <summary>
        /// Наименование документа
        /// </summary>
        [XmlElement(Order = 11)]
        public required string DocName { get; set; }
        /// <summary>
        /// Комментарий к документу
        /// </summary>
        [XmlElement(Order = 12)]
        public string? DocComment { get; set; } = string.Empty;
        /// <summary>
        /// Список хэшей прикрепленных файлов
        /// </summary>
        [XmlElement(Order = 13)]
        public List<string> ListFileHash { get; set; } = [];
    }
}
