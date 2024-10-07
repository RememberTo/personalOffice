using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entites.TestCFI
{
    /// <summary>
    /// Секция теста
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Количество вопросов
        /// </summary>
        [XmlAttribute("count")]
        public int Count { get; set; }
        /// <summary>
        /// Список вопросов
        /// </summary>
        [XmlElement("Question")]
        public required List<Question> Questions { get; set; }
    }
}
