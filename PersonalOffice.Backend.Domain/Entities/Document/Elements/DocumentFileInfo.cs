using Newtonsoft.Json;
using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entities.Document.Elements
{
    /// <summary>
    /// Информация о файле документа
    /// </summary>
    [Serializable]
    [XmlRoot("DocFileInfo")]
    public class DocumentFileInfo
    {
        /// <summary>
        /// Идентфикатор файла
        /// </summary>
        [JsonProperty("FileID")]
        public required string ID { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public required string Name { get; set; }
    }
}
