using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entites.TestCFI
{
    /// <summary>
    /// Базовое представление теста не квалифицированных инвесторов
    /// </summary>
    public class Root
    {
        /// <summary>
        /// Версия
        /// </summary>
        [XmlElement("Version")]
        public required string Version { get; set; }
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        [XmlElement("TestTypeID")]
        public byte TestId { get; set; }
        /// <summary>
        /// Наименование в родительном падеже
        /// </summary>
        [XmlElement("NameGenitiveCase")]
        public string? NameGenitiveCase { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [XmlElement("Description")]
        public string? Description { get; set; }
        /// <summary>
        /// Вопросы по самооценке
        /// </summary>
        [XmlElement("SelfEsteem")]
        public required Section SelfEsteem { get; set; }
        /// <summary>
        /// Вопросы на зание
        /// </summary>
        [XmlElement("Knowledge")]
        public required Section Knowledge { get; set; }
        /// <summary>
        /// Наименование файла
        /// </summary>
        [XmlIgnore]
        public required string FileName { get; set; }

        /// <summary>
        /// Создает клон объекта
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">При ошибках десериализации</exception>
        public Root Clone()
        {
            var tmpRoot = JsonConvert.SerializeObject(this);

            return JsonConvert.DeserializeObject<Root>(tmpRoot) ?? throw new InvalidCastException("Не удалось создать копию");
        }
    }
}
